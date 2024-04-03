import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import { App, RoutesNames } from "../constants";
import { useNavigate } from "react-router-dom";
import "./NavBar.css";

function NavBar() {
    const navigate = useNavigate();

    return (
        <>
            <Navbar expand="lg" bg="dark" data-bs-theme="dark" className="mb-5">
                <Container>
                    <Navbar.Brand className="linkHome" onClick={() => navigate(RoutesNames.HOME)}>
                        Inventory Management APP
                    </Navbar.Brand>
                    <Navbar.Toggle aria-controls="basic-navbar-nav" />
                    <Navbar.Collapse id="basic-navbar-nav">
                        <Nav className="me-auto">
                            <Nav.Link onClick={() => navigate(RoutesNames.WAREHOUSES_LIST)}>
                                Warehouses
                            </Nav.Link>
                            <Nav.Link onClick={() => navigate(RoutesNames.PRODUCTS_LIST)}>
                                Products
                            </Nav.Link>
                            <Nav.Link onClick={() => navigate(RoutesNames.EMPLOYEES_LIST)}>
                                Employees
                            </Nav.Link>
                            <Nav.Link onClick={() => navigate(RoutesNames.TRANSACTIONS_LIST)}>
                                Transactions
                            </Nav.Link>
                        </Nav>
                        <Nav>
                            <Nav.Link target="_blank" href={App.URL + "/swagger/index.html"}>
                                API Documentation
                            </Nav.Link>
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        </>
    );
}

export default NavBar;
