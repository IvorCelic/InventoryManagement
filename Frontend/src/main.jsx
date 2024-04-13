import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import App from "./App.jsx";
import { ErrorProvider } from "./components/ErrorContext.jsx";
import { AuthorizationProvider } from "./components/AuthorizationContext.jsx";

ReactDOM.createRoot(document.getElementById("root")).render(
    <React.StrictMode>
        <BrowserRouter>
            <ErrorProvider>
                <AuthorizationProvider>
                    <App />
                </AuthorizationProvider>
            </ErrorProvider>
        </BrowserRouter>
    </React.StrictMode>
);
