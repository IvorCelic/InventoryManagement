import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import { RoutesNames } from "../../constants";

export default function LocationsAdd(){

    return (
        <Container>
        <Container className="square border mt-5">
            <h2 className="mt-5 ms-5">Add new location</h2>
           <Form className="m-5">
                <Form.Group controlId="naziv">
                    <Form.Label>Name</Form.Label>
                    <Form.Control
                        placeholder="Location name"
                        type="text"
                        name="name"
                    />
                </Form.Group>
                <Form.Group controlId="trajanje">
                    <Form.Label>Description</Form.Label>
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