import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import { RoutesNames } from "../../constants";
import EmployeeService from "../../services/EmployeeService";

export default function EmployeesCreate() {
    const navigate = useNavigate();
    const entityName = "employee";

    async function addEmployee(entityName) {
        const response = await EmployeeService.add(entityName);
        if (response.ok) {
            navigate(RoutesNames.EMPLOYEES_LIST);
        } else {
            console.log(response);
            alert(response.message);
        }
    }

    function handleSubmit(error) {
        error.preventDefault();
        const data = new FormData(error.target);

        const entityName = {
            firstName: data.get("firstName"),
            lastName: data.get("lastName"),
            email: data.get("email"),
            password: data.get("password"),
        };

        addEmployee(entityName);
    }

    return (
        <Container>
            <Container className="square border mt-5">
                <h2 className="mt-5 ms-5">Add new {entityName}</h2>
                <Form className="m-5" onSubmit={handleSubmit}>
                    <Form.Group controlId="firstName">
                        <Form.Label>First name</Form.Label>
                        <Form.Control
                            placeholder="Employee's first name"
                            type="text"
                            name="firstName"
                            required
                        />
                    </Form.Group>
                    <Form.Group controlId="lastName">
                        <Form.Label className="pt-4">Last name</Form.Label>
                        <Form.Control
                            placeholder="Employee's last name"
                            type="text"
                            name="lastName"
                            required
                        />
                    </Form.Group>
                    <Form.Group controlId="email">
                        <Form.Label className="pt-4">Email</Form.Label>
                        <Form.Control
                            placeholder="Employee's email"
                            type="text"
                            name="email"
                            required
                        />
                    </Form.Group>
                    <Form.Group controlId="password">
                        <Form.Label className="pt-4">Password</Form.Label>
                        <Form.Control
                            placeholder="Employee's password"
                            type="password"
                            minLength={8}
                            name="password"
                            required
                        />
                    </Form.Group>
                    <Row className="mb-0 flex-column flex-sm-row">
                        <Col className="d-flex align-items-center mb-2 mb-sm-0">
                            <Link className="btn btn-danger myButton" to={RoutesNames.EMPLOYEES_LIST}>
                                Cancel
                            </Link>
                        </Col>
                        <Col className="d-flex align-items-center">
                            <Button className="myButton" variant="primary" type="submit">
                                Add {entityName}
                            </Button>
                        </Col>
                    </Row>
                </Form>
            </Container>
        </Container>
    );
}
