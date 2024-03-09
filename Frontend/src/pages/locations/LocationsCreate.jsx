import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import { RoutesNames } from "../../constants";
import LocationService from "../../services/LocationService";

export default function LocationsCreate(){
    const navigate = useNavigate();

    async function addLocation(location) {
        const response = await LocationService.addLocation(location);
        if (response.ok) {
            navigate(RoutesNames.LOCATIONS_LIST);
        }
        else {
            console.log(response);
            alert(response.message);
        }
    }

    function handleSubmit(error) {
        error.preventDefault();
        const data = new FormData(error.target)

        const location =
        {
            name: data.get('name'),
            description: data.get('description')
        };

        addLocation(location);
    }

    return (
        <Container>
        <Container className="square border mt-5">
            <h2 className="mt-5 ms-5">Add new location</h2>
           <Form
            className="m-5"
            onSubmit={handleSubmit}
            >
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
                <Row>
                    <Col>
                        <Link 
                        className="btn btn-danger myButton"
                        to={RoutesNames.LOCATIONS_LIST}>Cancel</Link>
                    </Col>
                    <Col>
                        <Button
                            className="myButton"
                            variant="primary"
                            type="submit"
                        >
                            Add location
                        </Button>
                    </Col>
                </Row>
           </Form>
        </Container>
        </Container>
    );

}