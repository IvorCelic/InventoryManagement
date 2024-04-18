import { Container, Form } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { RoutesNames } from "../../constants";
import EmployeeService from "../../services/EmployeeService";
import useError from "../../hooks/useError";
import ActionButtons from "../../components/ActionButtons";
import useLoading from "../../hooks/useLoading";

export default function EmployeesCreate() {
    const entityName = "employee";
    const navigate = useNavigate();
    const { showError } = useError();
    const { showLoading, hideLoading } = useLoading();

    async function addEmployee(entityName) {
        showLoading();
        const response = await EmployeeService.add("Employee", entityName);
        if (response.ok) {
            navigate(RoutesNames.EMPLOYEES_LIST);
            return;
        }
        showError(response.data);
        hideLoading();
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
                        />
                    </Form.Group>
                    <Form.Group controlId="lastName">
                        <Form.Label className="pt-4">Last name</Form.Label>
                        <Form.Control
                            placeholder="Employee's last name"
                            type="text"
                            name="lastName"
                        />
                    </Form.Group>
                    <Form.Group controlId="email">
                        <Form.Label className="pt-4">Email</Form.Label>
                        <Form.Control
                            placeholder="Employee's email"
                            type="text"
                            name="email"
                        />
                    </Form.Group>
                    <Form.Group controlId="password">
                        <Form.Label className="pt-4">Password</Form.Label>
                        <Form.Control
                            placeholder="Employee's password"
                            type="password"
                            name="password"
                        />
                    </Form.Group>
                    <ActionButtons
                        cancel={RoutesNames.EMPLOYEES_LIST}
                        action="Add employee"
                    />
                </Form>
            </Container>
        </Container>
    );
}
