import React from "react";
import ReactDOM from "react-dom/client";
import { BrowserRouter } from "react-router-dom";
import App from "./App.jsx";
import { ErrorProvider } from "./components/ErrorContext.jsx";

ReactDOM.createRoot(document.getElementById("root")).render(
    <React.StrictMode>
        <BrowserRouter>
            <ErrorProvider>
                <App />
            </ErrorProvider>
        </BrowserRouter>
    </React.StrictMode>
);
