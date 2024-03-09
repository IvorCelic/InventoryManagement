import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { Link, useNavigate, useParams } from "react-router-dom";
import { RoutesNames } from "../../constants";
import PersonService from "../../services/PersonService";
import { useEffect, useState } from "react";

export default function PersonsEdit(){
    const navigate = useNavigate();
    const routeParams = useParams();
    const [person, setPerson] = useState({});
    const entityName = 'person'

    async function fetchPerson() {
        await PersonService.getById(routeParams.id)
        .then((res) => {
            setPerson(res.data)
        })
        .catch((error) => {
            alert(error.message)
        });
    }
    
    useEffect(() => {
        fetchPerson();
    }, []);

    async function editPerson(entityName) {
        const response = await PersonService.edit(routeParams.id, entityName);
        if (response.ok) {
            navigate(RoutesNames.PERSONS_LIST);
        }
        else {
            console.log(response);
            alert(response.message);
        }
    }

    function handleSubmit(error) {
        error.preventDefault();
        const data = new FormData(error.target)

        const entityName =
        {
            firstName: data.get('firstName'),
            lastName: data.get('lastName'),
            email: data.get('email'),
            password: data.get('password')
        };

        editPerson(entityName);
    }

    return (
        <Container>
        <Container className="square border mt-5">
            <h2 className="mt-5 ms-5">Edit {entityName}</h2>
           <Form
            className="m-5" 
            onSubmit={handleSubmit}
            >
                <Form.Group controlId="firstName">
                    <Form.Label>First name</Form.Label>
                    <Form.Control
                        type="text"
                        defaultValue={person.firstName}
                        name="firstName"
                        required
                    />
                </Form.Group>
                <Form.Group controlId="lastName">
                    <Form.Label className="pt-4">Last name</Form.Label>
                    <Form.Control 
                        type="text"
                        defaultValue={person.lastName}
                        name="lastName"
                        required
                    />
                </Form.Group>
                <Form.Group controlId="email">
                    <Form.Label className="pt-4">Email</Form.Label>
                    <Form.Control 
                        type="text"
                        defaultValue={person.email}
                        name="email"
                        required
                    />
                </Form.Group>
                <Form.Group controlId="password">
                    <Form.Label className="pt-4">Password</Form.Label>
                    <Form.Control 
                        type="password"
                        minLength={8}
                        defaultValue={person.password}
                        name="password"
                        required
                    />
                </Form.Group>
                <Row className="mb-0 flex-column flex-sm-row">
                    <Col className="d-flex align-items-center mb-2 mb-sm-0">
                        <Link 
                        className="btn btn-danger myButton"
                        to={RoutesNames.PERSONS_LIST}>Cancel</Link>
                    </Col>
                    <Col className="d-flex align-items-center">
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