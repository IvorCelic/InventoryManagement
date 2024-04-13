import Container from "react-bootstrap/Container";
import Nav from "react-bootstrap/Nav";
import Navbar from "react-bootstrap/Navbar";
import { App, RoutesNames } from "../constants";
import { useNavigate } from "react-router-dom";
import "./NavBar.css";
import useAuthorization from "../hooks/useAuthorization";

function NavBar() {
    const navigate = useNavigate();
    const { logout, isLoggedIn } = useAuthorization();

    return (
        <>
            <Navbar expand="lg" bg="dark" data-bs-theme="dark" className="mb-5">
                <Container>
                    <Navbar.Brand className="linkHome" onClick={() => navigate(RoutesNames.HOME)}>
                        Inventory Management APP
                    </Navbar.Brand>
                    {isLoggedIn ? (
                        <>
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
                                    <Nav.Link
                                        onClick={() => navigate(RoutesNames.TRANSACTIONS_LIST)}
                                    >
                                        Transactions
                                    </Nav.Link>
                                </Nav>
                            </Navbar.Collapse>
                            <Navbar.Collapse className="justify-content-end">
                                <Nav>
                                    <Nav.Link onClick={logout}>Logout</Nav.Link>
                                    <Nav.Link
                                        target="_blank"
                                        href={App.URL + "/swagger/index.html"}
                                    >
                                        API Documentation
                                    </Nav.Link>
                                </Nav>
                            </Navbar.Collapse>
                        </>
                    ) : (
                        <>
                            <Navbar.Collapse className="justify-content-end">
                                <Nav>
                                    <Nav.Link onClick={() => navigate(RoutesNames.LOGIN)}>
                                        Login
                                    </Nav.Link>
                                </Nav>
                            </Navbar.Collapse>
                        </>
                    )}
                </Container>
            </Navbar>
        </>
    );
}

export default NavBar;
