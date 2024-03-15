import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { Link, useNavigate, useParams } from "react-router-dom";
import { RoutesNames } from "../../constants";
import LocationService from "../../services/WarehouseService";
import { useEffect, useState } from "react";

export default function LocationsEdit() {
    const navigate = useNavigate();
    const routeParams = useParams();
    const [location, setLocation] = useState({});
    const entityName = "location";

    async function fetchLocation() {
        await LocationService.getById(routeParams.id)
            .then((res) => {
                setLocation(res.data);
            })
            .catch((error) => {
                alert(error.message);
            });
    }

    useEffect(() => {
        fetchLocation();
    }, []);

    async function editLocation(entityName) {
        const response = await LocationService.edit(routeParams.id, entityName);
        if (response.ok) {
            navigate(RoutesNames.LOCATIONS_LIST);
        } else {
            console.log(response);
            alert(response.message);
        }
    }

    function handleSubmit(error) {
        error.preventDefault();
        const data = new FormData(error.target);

        const entityName = {
            name: data.get("name"),
            description: data.get("description"),
        };

        editLocation(entityName);
    }

    return (
        <Container>
            <Container className="square border mt-5">
                <h2 className="mt-5 ms-5">Edit location</h2>
                <Form className="m-5" onSubmit={handleSubmit}>
                    <Form.Group controlId="name">
                        <Form.Label>Name</Form.Label>
                        <Form.Control
                            type="text"
                            defaultValue={location.name}
                            name="name"
                        />
                    </Form.Group>
                    <Form.Group controlId="description">
                        <Form.Label className="pt-4">Description</Form.Label>
                        <Form.Control
                            type="text"
                            defaultValue={location.description}
                            name="description"
                        />
                    </Form.Group>
                    <Row>
                        <Col>
                            <Link
                                className="btn btn-danger myButton"
                                to={RoutesNames.LOCATIONS_LIST}
                            >
                                Cancel
                            </Link>
                        </Col>
                        <Col>
                            <Button
                                className="myButton"
                                variant="primary"
                                type="submit"
                            >
                                Save changes
                            </Button>
                        </Col>
                    </Row>
                </Form>
            </Container>
        </Container>
    );
}
