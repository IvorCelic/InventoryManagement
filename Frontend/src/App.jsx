import { Route, Routes } from "react-router-dom";
import { RoutesNames } from "./constants";

import Login from "./pages/Login";
import Home from "./pages/Home";
import NavBar from "./components/NavBar";

import "bootstrap/dist/css/bootstrap.min.css";
import "./App.css";

import Warehouses from "./pages/warehouses/Warehouses";
import WarehousesCreate from "./pages/warehouses/WarehousesCreate";
import WarehousesEdit from "./pages/warehouses/WarehousesEdit";

import Products from "./pages/products/Products";
import ProductsCreate from "./pages/products/ProductsCreate";
import ProductsEdit from "./pages/products/ProductsEdit";
import ProductDetails from "./pages/products/ProductDetails";

import Employees from "./pages/employees/Employees";
import EmployeesCreate from "./pages/employees/EmployeesCreate";
import EmployeesEdit from "./pages/employees/EmployeesEdit";

import Transactions from "./pages/transactions/Transactions";
import TransactionsCreate from "./pages/transactions/TransactionsCreate";
import TransactionsEdit from "./pages/transactions/TransactionsEdit";

import ErrorModal from "./components/ErrorModal";
import useError from "./hooks/useError";

function App() {
    const { showErrorModal, errors, hideError } = useError();

    return (
        <>
            <ErrorModal show={showErrorModal} errors={errors} onHide={hideError} />
            <NavBar />
            <Routes>
                <>
                    <Route path={RoutesNames.LOGIN} element={<Login />} />
                    <Route path={RoutesNames.HOME} element={<Home />} />

                    <Route path={RoutesNames.WAREHOUSES_LIST} element={<Warehouses />} />
                    <Route path={RoutesNames.WAREHOUSES_CREATE} element={<WarehousesCreate />} />
                    <Route path={RoutesNames.WAREHOUSES_EDIT} element={<WarehousesEdit />} />

                    <Route path={RoutesNames.EMPLOYEES_LIST} element={<Employees />} />
                    <Route path={RoutesNames.EMPLOYEES_CREATE} element={<EmployeesCreate />} />
                    <Route path={RoutesNames.EMPLOYEES_EDIT} element={<EmployeesEdit />} />

                    <Route path={RoutesNames.PRODUCTS_LIST} element={<Products />} />
                    <Route path={RoutesNames.PRODUCTS_CREATE} element={<ProductsCreate />} />
                    <Route path={RoutesNames.PRODUCTS_EDIT} element={<ProductsEdit />} />
                    <Route path={RoutesNames.PRODUCTS_DETAILS} element={<ProductDetails />} />

                    <Route path={RoutesNames.TRANSACTIONS_LIST} element={<Transactions />} />
                    <Route
                        path={RoutesNames.TRANSACTIONS_CREATE}
                        element={<TransactionsCreate />}
                    />
                    <Route path={RoutesNames.TRANSACTIONS_EDIT} element={<TransactionsEdit />} />
                </>
            </Routes>
        </>
    );
}

export default App;
