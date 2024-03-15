import { Route, Routes } from "react-router-dom";
import { RoutesNames } from "./constants";
import "bootstrap/dist/css/bootstrap.min.css";
import "./App.css";

import NavBar from "./components/NavBar";
import Login from "./pages/Login";
import Home from "./pages/Home";

import Warehouses from "./pages/warehouses/Warehouses";

import Products from "./pages/products/Products";

import Employees from "./pages/employees/Employees";
import EmployeesCreate from "./pages/employees/EmployeesCreate";
import EmployeesEdit from "./pages/employees/EmployeesEdit";

function App() {
    return (
        <>
            <NavBar />
            <Routes>
                <>
                    <Route path={RoutesNames.LOGIN} element={<Login />} />
                    <Route path={RoutesNames.HOME} element={<Home />} />

                    <Route path={RoutesNames.WAREHOUSES_LIST} element={<Warehouses />} />
                    <Route path={RoutesNames.WAREHOUSES_CREATE} element={<Warehouses />} />
                    <Route path={RoutesNames.WAREHOUSES_EDIT} element={<Warehouses />} />

                    <Route path={RoutesNames.EMPLOYEES_LIST} element={<Employees />} />
                    <Route path={RoutesNames.EMPLOYEES_CREATE} element={<EmployeesCreate />} />
                    <Route path={RoutesNames.EMPLOYEES_EDIT} element={<EmployeesEdit />} />

                    <Route path={RoutesNames.PRODUCTS_LIST} element={<Products />} />
                </>
            </Routes>
        </>
    );
}

export default App;
