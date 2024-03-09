import { useEffect, useState } from "react";
import { Button, Col, Container, Row, Table } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import { FaEdit, FaSearch, FaTrash } from "react-icons/fa";
import LocationService from "../../services/LocationService";
import { GrFormAdd } from "react-icons/gr";
import { RoutesNames } from "../../constants";

export default function Locations() {
    const [locations, setLocations] = useState();
    const navigate = useNavigate();

    async function fetchLocations() {
        await LocationService.getLocations()
            .then((res) => {
                setLocations(res.data);
            })
            .catch((e) => {
                alert(e);
            });
    }

    async function handleLocationDelete(id) {
        const response = await LocationService.deleteLocation(id)
        if (response.ok) {
            alert(response.message.data.message);
            fetchLocations();
        }
    }

    useEffect(() => {
        fetchLocations();
    }, []);

    return (
        <Container>
            <Container className="mt-5">
                <Row>
                    <Col lg="4" className="d-flex align-items-center">
                        <FaSearch />
                            <input
                                type="text"
                                className="border-0 border-bottom searchInput"
                                placeholder="Search"
                            />
                    </Col>
                    <Col className="d-flex align-items-center">
                        <Link to={RoutesNames.LOCATIONS_CREATE} className="btn btn-primary addButton">
                            <GrFormAdd size={22} /> Add new location
                        </Link>
                    </Col>
                </Row>
            </Container>
            <Table striped bordered hover responsive>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    {locations && locations.map((location, index) => (
                        <tr key={index}>
                            <td>{location.name}</td>
                            <td>{location.description}</td>
                            <td>
                                <Container
                                    className="d-flex justify-content-center"
                                >
                                    <Button
                                        variant="link"
                                        className="me-2 actionButton"
                                        onClick={() => { navigate(`/locations/${location.id}`) }}
                                    >
                                        <FaEdit
                                            size={25}
                                        />
                                    </Button>
                                    <Button
                                        className='link-danger actionButton'
                                        onClick={() => handleLocationDelete(location.id)}
                                    >
                                        <FaTrash
                                            size={25}
                                        />
                                    </Button>
                                </Container>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </Table>
        </Container>
    );
}
