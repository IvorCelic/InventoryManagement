import { Route, Routes } from "react-router-dom";
import { RoutesNames } from "./constants";
import "bootstrap/dist/css/bootstrap.min.css";
import "./App.css";

import NavBar from "./components/NavBar";
import Login from "./pages/Login";
import Home from "./pages/Home";

import Warehouses from "./pages/warehouses/Warehouses";
import WarehousesCreate from "./pages/warehouses/WarehousesCreate";
import WarehousesEdit from "./pages/warehouses/WarehousesEdit";

import Products from "./pages/products/Products";
import ProductsCreate from "./pages/products/ProductsCreate";
import ProductsEdit from "./pages/products/ProductsEdit";

import Employees from "./pages/employees/Employees";
import EmployeesCreate from "./pages/employees/EmployeesCreate";
import EmployeesEdit from "./pages/employees/EmployeesEdit";

import Transactions from "./pages/transactions/Transactions";
import TransactionItems from "./pages/transactions/transactionItems/TransactionItems";

function App() {
    return (
        <>
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

                    <Route path={RoutesNames.TRANSACTIONS_LIST} element={<Transactions />} />

                    <Route path={RoutesNames.TRANSACTIONITEMS_LIST} element={<TransactionItems />} />
                </>
            </Routes>
        </>
    );
}

export default App;
