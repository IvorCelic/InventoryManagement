import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import { RoutesNames } from "../../constants";
import LocationService from "../../services/WarehouseService";

export default function LocationsCreate() {
    const navigate = useNavigate();
    const entityName = "location";

    async function addLocation(entityName) {
        const response = await LocationService.add(entityName);
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

        addLocation(entityName);
    }

    return (
        <Container>
            <Container className="square border mt-5">
                <h2 className="mt-5 ms-5">Add new {entityName}</h2>
                <Form className="m-5" onSubmit={handleSubmit}>
                    <Form.Group controlId="name">
                        <Form.Label>Name</Form.Label>
                        <Form.Control
                            placeholder="Location name"
                            type="text"
                            name="name"
                        />
                    </Form.Group>
                    <Form.Group controlId="description">
                        <Form.Label className="pt-4">Description</Form.Label>
                        <Form.Control
                            placeholder="Location description"
                            type="text"
                            name="description"
                        />
                    </Form.Group>
                    <Row className="mb-0 flex-column flex-sm-row">
                        <Col className="d-flex align-items-center mb-2 mb-sm-0">
                            <Link
                                className="btn btn-danger myButton"
                                to={RoutesNames.LOCATIONS_LIST}
                            >
                                Cancel
                            </Link>
                        </Col>
                        <Col className="d-flex align-items-center">
                            <Button
                                className="myButton"
                                variant="primary"
                                type="submit"
                            >
                                Add {entityName}
                            </Button>
                        </Col>
                    </Row>
                </Form>
            </Container>
        </Container>
    );
}
