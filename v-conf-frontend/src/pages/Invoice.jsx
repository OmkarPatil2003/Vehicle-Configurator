/**
 * Purpose: Invoice Generation Page.
 * Displays order summary, tax details, and confirms the order.
 */
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
// eslint-disable-next-line no-unused-vars
import { motion, AnimatePresence } from "framer-motion";
import Button from "../components/ui/Button";
import Card from "../components/ui/Card";
import { Printer, CheckCircle, ArrowLeft } from "lucide-react";
import { invoiceService } from "../services/invoiceService";
import { defaultConfigService } from "../services/defaultConfigService";
import { useSelector, useDispatch } from "react-redux";
import { setDefaultComponents } from "../store/slices/configSlice";

const Invoice = () => {
    const navigate = useNavigate();
    const dispatch = useDispatch();

    // Redux State
    const {
        selectedModel,
        quantity,
        selectedOptions,
        defaultComponents,
        pricing
    } = useSelector((state) => state.config);

    const { user } = useSelector((state) => state.auth);

    const [isSubmitting, setIsSubmitting] = useState(false);
    const [showSuccess, setShowSuccess] = useState(false);
    const [countdown, setCountdown] = useState(10);
    // eslint-disable-next-line no-unused-vars
    const [loadingDefaults, setLoadingDefaults] = useState(false);

    // Redirect if no order data
    useEffect(() => {
        if (!selectedModel) {
            navigate("/welcome");
        }
    }, [selectedModel, navigate]);

    // Fetch defaults if missing (e.g. refresh)
    useEffect(() => {
        const fetchDefaults = async () => {
            if (selectedModel && (!defaultComponents || defaultComponents.length === 0)) {
                setLoadingDefaults(true);
                try {
                    const componentsList = await defaultConfigService.getDefaultConfig(selectedModel.id);
                    dispatch(setDefaultComponents(componentsList));
                } catch (error) {
                    console.error("Failed to load default components for invoice", error);
                } finally {
                    setLoadingDefaults(false);
                }
            }
        };
        fetchDefaults();
    }, [selectedModel, defaultComponents, dispatch]);

    useEffect(() => {
        if (showSuccess && countdown > 0) {
            const timer = setTimeout(() => setCountdown(c => c - 1), 1000);
            return () => clearTimeout(timer);
        } else if (showSuccess && countdown === 0) {
            navigate("/");
        }
    }, [showSuccess, countdown, navigate]);

    const [invoiceNumber] = useState(() => Math.floor(100000 + Math.random() * 900000));
    const [validDate] = useState(() => new Date(Date.now() + 7 * 24 * 60 * 60 * 1000).toLocaleDateString());
    const currentDate = new Date().toLocaleDateString();

    const handleConfirmOrder = async () => {
        setIsSubmitting(true);
        try {
            const userId = user ? user.id : 1;
            const customerDetail = user ? `${user.companyName || user.username}` : "Guest Customer";

            const alternates =  Object.values(selectedOptions).map(opt => opt.optionId).filter(id => id != null);

            const invoiceDto = {
                userId: userId,
                modelId: selectedModel.id,
                qty: quantity,
                customerDetail: customerDetail,
                alternates: alternates
            };

            await invoiceService.confirmOrder(invoiceDto);
            setShowSuccess(true);
        } catch (error) {
            console.error(error);
            alert("Failed to confirm order. Please try again.");
            setIsSubmitting(false);
        }
    };

    if (!selectedModel) return null;

    // Pricing Calcs
    const configuredTotalPerUnit = pricing.totalPerUnit;
    const subTotal = configuredTotalPerUnit * quantity; // Base + Addons
    const taxes = subTotal * 0.18;
    const finalTotal = subTotal + taxes;

    // Generate Invoice Items List (Merged Logic)
    const renderComponentList = () => {
        if (!defaultComponents) return [];
        return defaultComponents.map((defComp) => {
            const override = selectedOptions[defComp.id];
            if (override) {
                return {
                    name: override.label || override.subType || "Upgrade",
                    price: override.price,
                    isUpgrade: true
                };
            }
            return {
                name: defComp.name,
                isUpgrade: false
            };
        });
    };

    const componentList = renderComponentList();

    return (
        <div className="py-12 bg-slate-50 dark:bg-slate-950 min-h-screen relative">

            {/* Success Overlay */}
            <AnimatePresence>
                {showSuccess && (
                    <motion.div
                        initial={{ opacity: 0 }}
                        animate={{ opacity: 1 }}
                        exit={{ opacity: 0 }}
                        className="fixed inset-0 z-50 flex items-center justify-center bg-slate-900/90 backdrop-blur-sm"
                    >
                        <motion.div
                            initial={{ scale: 0.8, opacity: 0 }}
                            animate={{ scale: 1, opacity: 1 }}
                            className="bg-white dark:bg-slate-900 p-8 rounded-2xl shadow-2xl max-w-md w-full text-center border border-slate-200 dark:border-slate-800"
                        >
                            <div className="w-20 h-20 bg-green-100 dark:bg-green-900/30 rounded-full flex items-center justify-center mx-auto mb-6">
                                <CheckCircle className="h-10 w-10 text-green-600 dark:text-green-400" />
                            </div>
                            <h2 className="text-3xl font-bold text-slate-900 dark:text-white mb-2">Order Successful!</h2>
                            <p className="text-slate-500 mb-8">
                                Your vehicle configuration has been successfully submitted to our production team.<br />
                                An invoice has been sent to your registered email.
                            </p>
                            <div className="bg-slate-50 dark:bg-slate-800 rounded-lg p-4 mb-6">
                                <p className="text-sm text-slate-400 uppercase tracking-wider mb-1">Estimated Delivery</p>
                                <p className="font-semibold text-slate-800 dark:text-white">4-6 Weeks</p>
                            </div>
                            <p className="text-sm text-slate-400">
                                Redirecting to home in <span className="text-blue-500 font-bold">{countdown}</span> seconds...
                            </p>
                        </motion.div>
                    </motion.div>
                )}
            </AnimatePresence>

            <div className={`max-w-4xl mx-auto px-4 transition-all duration-500 ${showSuccess ? 'blur-sm scale-95 opacity-50' : ''}`}>
                <div className="flex justify-between items-center mb-8">
                    <Button variant="ghost" onClick={() => navigate("/configure/" + selectedModel.id)} className="gap-2 pl-0 hover:bg-transparent hover:text-blue-600">
                        <ArrowLeft className="h-4 w-4" /> Back to Configuration
                    </Button>
                    <Button variant="outline" className="gap-2" onClick={() => window.print()}>
                        <Printer className="h-4 w-4" /> Print Invoice
                    </Button>
                </div>

                <Card className="p-0 overflow-hidden bg-white dark:bg-slate-900 shadow-xl printable">
                    {/* Invoice Header */}
                    <div className="p-8 bg-slate-900 text-white relative overflow-hidden">
                        <div className="relative z-10 flex justify-between items-start">
                            <div>
                                <h1 className="text-3xl font-bold mb-1">INVOICE</h1>
                                <p className="text-slate-400 text-sm">#INV-{invoiceNumber}</p>
                            </div>
                            <div className="text-right">
                                <h2 className="text-xl font-bold tracking-tight">V-CONF <span className="text-blue-400">Automotive</span></h2>
                                <p className="text-xs text-slate-400 mt-1">Premium Vehicle Solutions</p>
                            </div>
                        </div>
                        {/* Decorative background element */}
                        <div className="absolute top-0 right-0 -mt-10 -mr-10 w-64 h-64 bg-blue-500 rounded-full mix-blend-multiply filter blur-3xl opacity-20 animate-blob"></div>
                    </div>

                    <div className="p-8">
                        {/* Bill To / Date */}
                        <div className="grid md:grid-cols-2 gap-8 mb-8">
                            <div>
                                <h3 className="text-xs font-bold text-slate-400 uppercase tracking-widest mb-3">Billed To</h3>
                                {user ? (
                                    <>
                                        <p className="font-bold text-base mb-0.5 text-slate-900 dark:text-white">{user.companyName}</p>
                                        <p className="text-sm text-slate-500">Authorized Dealer</p>
                                        <p className="text-sm text-slate-500">{user.email || user.username}</p>
                                    </>
                                ) : (
                                    <>
                                        <p className="font-bold text-base mb-0.5">Guest Customer</p>
                                        <p className="text-sm text-slate-500">Retail Purchase</p>
                                    </>
                                )}
                            </div>
                            <div className="text-right md:text-right">
                                <h3 className="text-xs font-bold text-slate-400 uppercase tracking-widest mb-3">Order Details</h3>
                                <div className="space-y-1">
                                    <div className="flex justify-between md:justify-end gap-4 text-sm">
                                        <span className="text-slate-500">Date Issued:</span>
                                        <span className="font-medium text-slate-900 dark:text-white">{currentDate}</span>
                                    </div>
                                    <div className="flex justify-between md:justify-end gap-4 text-sm">
                                        <span className="text-slate-500">Valid Until:</span>
                                        <span className="font-medium text-slate-900 dark:text-white">{validDate}</span>
                                    </div>
                                </div>
                            </div>
                        </div>

                        {/* Component Details Table */}
                        <div className="relative overflow-hidden rounded-lg border border-slate-200 dark:border-slate-800 mb-8">
                            <table className="w-full text-left text-sm">
                                <thead className="bg-slate-50 dark:bg-slate-800/50 border-b border-slate-200 dark:border-slate-700">
                                    <tr>
                                        <th className="py-3 px-6 font-semibold text-slate-900 dark:text-white uppercase tracking-wider text-xs">Item Description</th>
                                        <th className="py-3 px-6 font-semibold text-slate-900 dark:text-white text-right uppercase tracking-wider text-xs">Unit Price</th>
                                        <th className="py-3 px-6 font-semibold text-slate-900 dark:text-white text-right uppercase tracking-wider text-xs">Qty</th>
                                        <th className="py-3 px-6 font-semibold text-slate-900 dark:text-white text-right uppercase tracking-wider text-xs">Amount</th>
                                    </tr>
                                </thead>
                                <tbody className="divide-y divide-slate-100 dark:divide-slate-800">
                                    <tr className="bg-white dark:bg-slate-900">
                                        <td className="py-6 px-6">
                                            {/* Main Vehicle Title */}
                                            <div className="mb-4">
                                                <p className="font-bold text-slate-900 dark:text-white text-lg">
                                                    {selectedModel.manufacturer ? selectedModel.manufacturer.name : ""} {selectedModel.name}
                                                </p>
                                                <div className="flex items-center gap-2 mt-1">
                                                    <span className="px-2 py-0.5 rounded text-[10px] font-bold bg-blue-100 text-blue-700 dark:bg-blue-900/30 dark:text-blue-300 uppercase tracking-wide">
                                                        {selectedModel.segment ? selectedModel.segment.name : "Vehicle"}
                                                    </span>
                                                </div>
                                            </div>

                                            {/* Component Breakdown List (Vertical) */}
                                            <div className="bg-slate-50 dark:bg-slate-800/50 rounded-lg p-4 border border-slate-100 dark:border-slate-700/50">
                                                <p className="text-xs font-bold text-slate-400 uppercase tracking-widest mb-3">Configuration Details</p>
                                                <div className="flex flex-col gap-y-3">
                                                    {componentList.map((comp, idx) => (
                                                        <div key={idx} className="flex justify-between items-start text-sm group border-b border-slate-200/50 dark:border-slate-700/50 pb-2 last:border-0 last:pb-0">
                                                            <div className="flex items-start gap-3 flex-1 mr-4">
                                                                <div className={`mt-1.5 w-1.5 h-1.5 rounded-full shrink-0 ${comp.isUpgrade ? 'bg-blue-500' : 'bg-slate-300 dark:bg-slate-600'}`}></div>
                                                                <span className={`${comp.isUpgrade ? 'text-slate-900 dark:text-white font-medium' : 'text-slate-600 dark:text-slate-400'} leading-snug`}>
                                                                    {comp.name}
                                                                </span>
                                                            </div>
                                                            <div className="shrink-0 text-right">
                                                                {comp.isUpgrade ? (
                                                                    <span className="text-[11px] font-semibold text-blue-600 dark:text-blue-400 bg-blue-50 dark:bg-blue-900/20 px-2 py-0.5 rounded border border-blue-100 dark:border-blue-900/30">
                                                                        + ₹{comp.price.toLocaleString()}
                                                                    </span>
                                                                ) : (
                                                                    <span className="text-[10px] text-slate-400 uppercase tracking-wider font-medium bg-slate-100 dark:bg-slate-700/50 px-2 py-0.5 rounded">Standard</span>
                                                                )}
                                                            </div>
                                                        </div>
                                                    ))}
                                                </div>
                                            </div>
                                        </td>

                                        {/* Pricing Columns - Aligned to Top */}
                                        <td className="py-6 px-6 text-right align-top">
                                            <span className="font-medium text-slate-900 dark:text-white block">
                                                ₹ {pricing.totalPerUnit.toLocaleString()}
                                            </span>
                                            {pricing.addOnsTotal > 0 && (
                                                <span className="text-xs text-slate-500 block mt-1">
                                                    (Base: ₹{pricing.basePrice.toLocaleString()})
                                                </span>
                                            )}
                                        </td>
                                        <td className="py-6 px-6 text-right align-top text-slate-600 dark:text-slate-400">
                                            {quantity}
                                        </td>
                                        <td className="py-6 px-6 text-right align-top font-bold text-slate-900 dark:text-white text-lg">
                                            ₹ {(pricing.totalPerUnit * quantity).toLocaleString()}
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                        {/* Totals Section */}
                        <div className="flex justify-end">
                            <div className="w-full max-w-sm">
                                <div className="space-y-4 py-4 border-b border-slate-200 dark:border-slate-800">
                                    <div className="flex justify-between text-slate-600 dark:text-slate-400">
                                        <span>Subtotal</span>
                                        <span>₹ {subTotal.toLocaleString()}</span>
                                    </div>
                                    <div className="flex justify-between text-slate-600 dark:text-slate-400">
                                        <span>Estimated Tax (18%)</span>
                                        <span>₹ {taxes.toLocaleString()}</span>
                                    </div>
                                </div>
                                <div className="flex justify-between items-center py-4">
                                    <span className="font-bold text-xl text-slate-900 dark:text-white">Total Buying Price</span>
                                    <span className="font-bold text-3xl text-blue-600">₹ {finalTotal.toLocaleString()}</span>
                                </div>
                            </div>
                        </div>

                        {/* Action Buttons */}
                        <div className="mt-12 text-right">
                            <Button
                                size="lg"
                                className="w-full md:w-auto px-8 h-14 text-lg bg-blue-600 hover:bg-blue-700 shadow-lg shadow-blue-600/30"
                                onClick={handleConfirmOrder}
                                disabled={isSubmitting}
                            >
                                {isSubmitting ? (
                                    <>
                                        <div className="w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin mr-3"></div>
                                        Processing Order...
                                    </>
                                ) : (
                                    <>
                                        <CheckCircle className="mr-2 h-5 w-5" />
                                        Confirm & Submit Order
                                    </>
                                )}
                            </Button>
                        </div>
                    </div>
                </Card>
            </div>
        </div>
    );
};

export default Invoice;
