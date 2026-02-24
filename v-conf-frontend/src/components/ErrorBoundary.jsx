/**
 * Purpose: Error Boundary Wrapper.
 * Catches JavaScript errors anywhere in the child component tree,
 * logs those errors, and displays a fallback UI instead of the component tree that crashed.
 */
import { Component } from "react";
// We can't use hooks like useNavigate here easily without a wrapper, 
// so we'll just render the GlobalError component directly or redirect.
// Rendering directly is better for UX as it keeps context.

// However, we need to pass the error message to the GlobalError component usually via URL if redirecting,
// or props if rendering. Let's create a simple inline style or import GlobalError.
// But GlobalError is a page component that might use hooks (useSearchParams).
// So we should wrap GlobalError or just direct import to render, BUT GlobalError uses hooks.
// Class components cannot Render a hook-using component safely if that component expects context 
// that might be broken, but useSearchParams comes from Router context which should be outside ErrorBoundary normally.

// Let's implement a simple fallback UI here or try to redirect.
// Actually, standard practice is to render a Fallback Component.
import GlobalError from "../pages/GlobalError";

class ErrorBoundary extends Component {
    constructor(props) {
        super(props);
        this.state = { hasError: false, error: null };
    }

    static getDerivedStateFromError(error) {
        // Update state so the next render will show the fallback UI.
        return { hasError: true, error };
    }

    componentDidCatch(error, errorInfo) {
        // You can also log the error to an error reporting service
        console.error("ErrorBoundary caught an error:", error, errorInfo);
    }

    render() {
        if (this.state.hasError) {
            // You can render any custom fallback UI
            // We can try to render GlobalError. 
            // Note: If GlobalError uses router hooks, ErrorBoundary must be inside Router.
            // In App.jsx, we will place ErrorBoundary INSIDE BrowserRouter.

            // We can mock the search params prop or context if needed, 
            // but simpler to just show a generic error if hooks fail.
            // Let's assume GlobalError handles missing params gracefully (it does: Check `message` is null friendly).
            // We can't pass 'message' via URL params here easily without navigation.
            // So we might need to modify GlobalError to accept props OR just show generic.

            // Actually, let's just modify GlobalError to accept `error` prop too!
            // But for now, let's keep it simple.
            return (
                <div className="min-h-screen flex items-center justify-center bg-slate-50 dark:bg-slate-900 p-4">
                    <div className="text-center">
                        <h1 className="text-2xl font-bold text-red-600 mb-2">Something went wrong.</h1>
                        <p className="text-slate-500 mb-4">{this.state.error?.toString()}</p>
                        <button
                            onClick={() => window.location.replace("/")}
                            className="px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
                        >
                            Return Home
                        </button>
                    </div>
                </div>
            );

            // Better: Redirect to /error?message=... ? 
            // We can't easily Redirect in render method of class without conditional rendering a Navigate component.
            // And Navigate component might cause loop if error persists.

            // BEST APPROACH: Render a Safe Fallback.
        }

        return this.props.children;
    }
}

export default ErrorBoundary;
