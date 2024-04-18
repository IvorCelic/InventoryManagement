import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import App from "./App.jsx";
import { ErrorProvider } from "./components/ErrorContext.jsx";
import { AuthorizationProvider } from "./components/AuthorizationContext.jsx";
import { LoadingProvider } from "./components/LoadingContext.jsx";

ReactDOM.createRoot(document.getElementById("root")).render(
    <React.StrictMode>
        <BrowserRouter>
            <ErrorProvider>
                <LoadingProvider>
                    <AuthorizationProvider>
                        <App />
                    </AuthorizationProvider>
                </LoadingProvider>
            </ErrorProvider>
        </BrowserRouter>
    </React.StrictMode>
);
