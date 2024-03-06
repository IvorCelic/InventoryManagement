import Container from 'react-bootstrap/Container';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import { RoutesNames } from '../constants';
import { useNavigate } from 'react-router-dom';
import './NavBar.css';

function NavBar() {
    const navigate = useNavigate();

    return (
        <>
            <Navbar bg="dark" data-bs-theme="dark">
                <Container>
                    <Navbar.Brand
                        className="linkHome"
                        onClick={() => navigate(RoutesNames.HOME)}
                    >
                        Inventory Management APP
                    </Navbar.Brand>
                    <Nav className="me-auto">
                        <Nav.Link
                            onClick={() => navigate(RoutesNames.LOCATIONS_LIST)}
                        >
                            Locations
                        </Nav.Link>
                        <Nav.Link
                            onClick={() => navigate(RoutesNames.PRODUCTS_LIST)}
                        >
                            Products
                        </Nav.Link>
                        <Nav.Link
                            onClick={() => navigate(RoutesNames.PERSONS_LIST)}
                        >
                            Persons
                        </Nav.Link>
                    </Nav>
                    <Nav>
                        <Nav.Link target="_blank" href="https://ivorcelic-001-site1.htempurl.com/swagger/index.html">API Documentation</Nav.Link>
                    </Nav>
                </Container>
            </Navbar>
        </>
    );
}

export default NavBar;