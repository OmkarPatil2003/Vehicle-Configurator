/**
 * Purpose: Global Error Page (500).
 * Displayed when the application encounters a critical error or crash.
 */
import { useEffect } from "react";
import { Link, useSearchParams } from "react-router-dom";
import { motion } from "framer-motion";
import Button from "../components/ui/Button";
import { RefreshCw, Home, AlertTriangle, LifeBuoy } from "lucide-react";
import Card from "../components/ui/Card";

const GlobalError = () => {
    const [searchParams] = useSearchParams();
    const message = searchParams.get("message");

    // Optional: Log error to an external service on mount
    useEffect(() => {
        // console.error("Global Error Page Mounted:", message);
    }, [message]);

    return (
        <div className="min-h-screen bg-slate-50 dark:bg-slate-950 flex flex-col items-center justify-center p-6 relative overflow-hidden">

            {/* Ambient Background Effects */}
            <div className="absolute top-0 left-0 w-full h-full overflow-hidden pointer-events-none">
                <div className="absolute -top-24 -left-24 w-96 h-96 bg-red-500/10 rounded-full blur-3xl animate-pulse"></div>
                <div className="absolute top-1/2 right-0 w-80 h-80 bg-orange-500/10 rounded-full blur-3xl mix-blend-multiply"></div>
                <div className="absolute bottom-0 left-1/3 w-64 h-64 bg-pink-500/10 rounded-full blur-3xl animate-pulse" style={{ animationDuration: '4s' }}></div>
            </div>

            <motion.div
                initial={{ opacity: 0, scale: 0.95 }}
                animate={{ opacity: 1, scale: 1 }}
                transition={{ duration: 0.4 }}
                className="w-full max-w-lg z-10"
            >
                <Card className="border-0 shadow-2xl bg-white/80 dark:bg-slate-900/80 backdrop-blur-xl p-0 overflow-hidden ring-1 ring-slate-200 dark:ring-slate-800">

                    {/* Error Header */}
                    <div className="bg-red-50 dark:bg-red-900/20 p-8 flex flex-col items-center justify-center border-b border-red-100 dark:border-red-900/50">
                        <motion.div
                            initial={{ rotate: -10, scale: 0.8 }}
                            animate={{ rotate: 0, scale: 1 }}
                            transition={{ type: "spring", stiffness: 200, delay: 0.2 }}
                            className="w-20 h-20 bg-red-100 dark:bg-red-800/30 text-red-600 dark:text-red-400 rounded-full flex items-center justify-center mb-4 shadow-inner"
                        >
                            <AlertTriangle className="w-10 h-10" />
                        </motion.div>
                        <h1 className="text-2xl md:text-3xl font-bold text-slate-800 dark:text-slate-100 text-center">
                            Something went wrong
                        </h1>
                    </div>

                    {/* Error Content */}
                    <div className="p-8 text-center space-y-6">
                        <p className="text-slate-600 dark:text-slate-400 leading-relaxed">
                            We encountered an unexpected error while processing your request.
                            Our team has been notified. Please try refreshing the page.
                        </p>

                        {message && (
                            <div className="bg-slate-100 dark:bg-slate-950 p-4 rounded-lg border border-slate-200 dark:border-slate-800 text-left">
                                <p className="text-xs font-mono text-slate-500 uppercase tracking-wider mb-1">Error Details:</p>
                                <p className="text-sm font-mono text-slate-700 dark:text-red-400 break-words">
                                    {decodeURIComponent(message)}
                                </p>
                            </div>
                        )}

                        <div className="flex flex-col sm:flex-row gap-3 pt-2">
                            <Button
                                onClick={() => window.location.reload()}
                                className="flex-1 justify-center gap-2 bg-blue-600 hover:bg-blue-700 shadow-lg shadow-blue-600/20"
                                size="lg"
                            >
                                <RefreshCw className="w-4 h-4" />
                                Refresh Page
                            </Button>

                            <Link to="/" className="flex-1">
                                <Button
                                    variant="outline"
                                    className="w-full justify-center gap-2 border-slate-300 dark:border-slate-700 hover:bg-slate-50 dark:hover:bg-slate-800"
                                    size="lg"
                                >
                                    <Home className="w-4 h-4" />
                                    Go Home
                                </Button>
                            </Link>
                        </div>

                        <div className="pt-6 border-t border-slate-100 dark:border-slate-800">
                            <a href="mailto:support@vconf.com" className="inline-flex items-center text-sm text-slate-400 hover:text-blue-600 transition-colors gap-1.5">
                                <LifeBuoy className="w-4 h-4" />
                                Contact Support
                            </a>
                        </div>
                    </div>
                </Card>
            </motion.div>

            <div className="mt-8 text-center text-slate-400 text-xs z-10">
                &copy; {new Date().getFullYear()} Vehicle Configurator System. All rights reserved.
            </div>
        </div>
    );
};

export default GlobalError;
