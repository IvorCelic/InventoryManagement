import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { Link, useNavigate, useParams } from "react-router-dom";
import { RoutesNames } from "../../constants";
import LocationService from "../../services/LocationService";
import { useEffect, useState } from "react";

export default function LocationsCreate(){
    const navigate = useNavigate();
    const routeParams = useParams();
    const [location, setLocation] = useState({});

    async function fetchLocation() {
        await LocationService.getById(routeParams.id)
        .then((res) => {
            setLocation(res.data)
        })
        .catch((error) => {
            alert(error.message)
        });
    }
    
    useEffect(() => {
        fetchLocation();
    }, []);

    async function editLocation(location) {
        const response = await LocationService.editLocation(routeParams.id, location);
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

        editLocation(location);
    }

    return (
        <Container>
        <Container className="square border mt-5">
            <h2 className="mt-5 ms-5">Edit location</h2>
           <Form
            className="m-5"
            onSubmit={handleSubmit}
            >
                <Form.Group controlId="name">
                    <Form.Label>Name</Form.Label>
                    <Form.Control
                        type="text"
                        defaultValue={location.name}
                        name="name"
                    />
                </Form.Group>
                <Form.Group controlId="description">
                    <Form.Label>Description</Form.Label>
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
                        to={RoutesNames.LOCATIONS_LIST}>Cancel</Link>
                    </Col>
                    <Col>
                        <Button
                            className="myButton"
                            variant="primary"
                            type="submit"
                        >
                            Edit location
                        </Button>
                    </Col>
                </Row>
           </Form>
        </Container>
        </Container>
    );

}