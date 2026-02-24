/**
 * Purpose: Main application component.
 * Handles client-side routing, global providers (I18n), and protected route logic.
 */
import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Suspense, lazy } from "react"; // Added Suspense and lazy for Lazy Loading

// Layouts & HOCs
import RootLayout from "./layouts/RootLayout";
import ProtectedRoute from "./components/ProtectedRoute";
import ScrollToTop from "./components/ScrollToTop";

// Context
import { I18nProvider } from "./context/I18nContext";

// Lazy Loading Implemented Here for Auth Pages
const Login = lazy(() => import("./pages/Login"));
const Register = lazy(() => import("./pages/Register"));
const ForgotPassword = lazy(() => import("./pages/ForgotPassword"));

// Lazy Loading Implemented Here for Core Pages
const Home = lazy(() => import("./pages/Home"));
const Contact = lazy(() => import("./pages/Contact"));
const Welcome = lazy(() => import("./pages/Welcome"));
const DefaultConfig = lazy(() => import("./pages/DefaultConfig"));
const ModifyConfig = lazy(() => import("./pages/ModifyConfig"));
const Invoice = lazy(() => import("./pages/Invoice"));

// Lazy Loading Implemented Here for Admin/Util Pages
const ExcelUpload = lazy(() => import("./pages/ExcelUpload"));
const NotFound = lazy(() => import("./pages/NotFound"));
const GlobalError = lazy(() => import("./pages/GlobalError")); // Added GlobalError

import ErrorBoundary from "./components/ErrorBoundary"; // Added ErrorBoundary

// Loading Fallback Component
const LoadingFallback = () => (
  <div className="flex items-center justify-center min-h-screen bg-slate-50 dark:bg-slate-900">
    <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
  </div>
);

function App() {
  return (
    <I18nProvider>
      <BrowserRouter>
        <ScrollToTop />
        {/* Error Boundary for Frontend Crashes */}
        <ErrorBoundary>
          {/* Suspense Wrapper for Lazy Loading */}
          <Suspense fallback={<LoadingFallback />}>
            <Routes>
              <Route path="/error" element={<GlobalError />} /> {/* Global Route */}
              <Route path="/" element={<RootLayout />}>

                <Route index element={<Home />} />
                <Route path="contact" element={<Contact />} />
                <Route path="login" element={<Login />} />
                <Route path="forgot-password" element={<ForgotPassword />} />
                <Route path="register" element={<Register />} />
                <Route path="welcome" element={
                  <ProtectedRoute>
                    <Welcome />
                  </ProtectedRoute>
                } />
                <Route path="configurator/:modelId" element={
                  <ProtectedRoute>
                    <DefaultConfig />
                  </ProtectedRoute>
                } />
                <Route path="configure/:id" element={
                  <ProtectedRoute>
                    <ModifyConfig />
                  </ProtectedRoute>
                } />
                <Route path="invoice" element={
                  <ProtectedRoute>
                    <Invoice />
                  </ProtectedRoute>
                } />
                <Route path="admin/upload" element={
                  <ProtectedRoute>
                    <ExcelUpload />
                  </ProtectedRoute>
                } />
                <Route path="*" element={<NotFound />} />
              </Route>
            </Routes>
          </Suspense>
        </ErrorBoundary>
      </BrowserRouter>
    </I18nProvider>
  );
}

export default App;
