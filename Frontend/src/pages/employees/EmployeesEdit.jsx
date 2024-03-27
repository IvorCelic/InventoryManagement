import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { Link, useNavigate, useParams } from "react-router-dom";
import { RoutesNames } from "../../constants";
import EmployeeService from "../../services/EmployeeService";
import { useEffect, useState } from "react";

export default function EmployeesEdit() {
    const navigate = useNavigate();
    const routeParams = useParams();
    const [employee, setEmployee] = useState({});
    const entityName = "employee";

    async function fetchEmployee() {
        await EmployeeService.getById(routeParams.id)
            .then((res) => {
                setEmployee(res.data);
            })
            .catch((error) => {
                alert(error.message);
            });
    }

    useEffect(() => {
        fetchEmployee();
    }, []);

    async function editEmployee(entityName) {
        const response = await EmployeeService.edit(routeParams.id, entityName);
        if (response.ok) {
            navigate(RoutesNames.EMPLOYEES_LIST);
        } else {
            console.log(response);
            alert(response.message);
        }
    }

    function handleSubmit(event) {
        event.preventDefault();
        const data = new FormData(event.target);

        const entityName = {
            firstName: data.get("firstName"),
            lastName: data.get("lastName"),
            email: data.get("email"),
            password: data.get("password"),
        };

        editEmployee(entityName);
    }

    return (
        <Container>
            <Container className="square border mt-5">
                <h2 className="mt-5 ms-5">Edit {entityName}</h2>
                <Form className="m-5" onSubmit={handleSubmit}>
                    <Form.Group controlId="firstName">
                        <Form.Label>First name</Form.Label>
                        <Form.Control
                            type="text"
                            defaultValue={employee.firstName}
                            name="firstName"
                            required
                        />
                    </Form.Group>
                    <Form.Group controlId="lastName">
                        <Form.Label className="pt-4">Last name</Form.Label>
                        <Form.Control
                            type="text"
                            defaultValue={employee.lastName}
                            name="lastName"
                            required
                        />
                    </Form.Group>
                    <Form.Group controlId="email">
                        <Form.Label className="pt-4">Email</Form.Label>
                        <Form.Control
                            type="text"
                            defaultValue={employee.email}
                            name="email"
                            required
                        />
                    </Form.Group>
                    <Form.Group controlId="password">
                        <Form.Label className="pt-4">Password</Form.Label>
                        <Form.Control
                            type="password"
                            minLength={8}
                            defaultValue={employee.password}
                            name="password"
                            required
                        />
                    </Form.Group>
                    <Row className="mb-0 flex-column flex-sm-row">
                        <Col className="d-flex align-items-center mb-2 mb-sm-0">
                            <Link
                                className="btn btn-danger myButton"
                                to={RoutesNames.EMPLOYEES_LIST}
                            >
                                Cancel
                            </Link>
                        </Col>
                        <Col className="d-flex align-items-center">
                            <Button className="myButton" variant="primary" type="submit">
                                Save changes
                            </Button>
                        </Col>
                    </Row>
                </Form>
            </Container>
        </Container>
    );
}
