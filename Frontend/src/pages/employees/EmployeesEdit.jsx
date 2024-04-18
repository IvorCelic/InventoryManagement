import { useEffect, useState } from "react";
import { Container, Form } from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import { RoutesNames } from "../../constants";
import EmployeeService from "../../services/EmployeeService";
import useError from "../../hooks/useError";
import ActionButtons from "../../components/ActionButtons";
import useLoading from "../../hooks/useLoading";

export default function EmployeesEdit() {
    const navigate = useNavigate();
    const routeParams = useParams();
    const [employee, setEmployee] = useState({});
    const entityName = "employee";
    const { showError } = useError();
    const [showModal, setShowModal] = useState();
    const { showLoading, hideLoading } = useLoading();

    async function fetchEmployee() {
        showLoading();
        const response = await EmployeeService.getById("Employee", routeParams.id);
        if (!response.ok) {
            showError(response.data);
            navigate(RoutesNames.EMPLOYEES_LIST);
            return;
        }
        setEmployee(response.data);
        setShowModal(false);
        hideLoading();
    }

    useEffect(() => {
        fetchEmployee();
    }, []);

    async function editEmployee(entityName) {
        showLoading();
        const response = await EmployeeService.edit("Employee", routeParams.id, entityName);
        if (response.ok) {
            hideLoading(); 
            // console.log(response.data);
            navigate(RoutesNames.EMPLOYEES_LIST);
            return;
        }
        showError(response.data);
        hideLoading();
    }

    function handleSubmit(event) {
        event.preventDefault();
        const data = new FormData(event.target);

        editEmployee({
            firstName: data.get("firstName"),
            lastName: data.get("lastName"),
            email: data.get("email"),
            password: data.get("password"),
        });
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
                        />
                    </Form.Group>
                    <Form.Group controlId="lastName">
                        <Form.Label className="pt-4">Last name</Form.Label>
                        <Form.Control
                            type="text"
                            defaultValue={employee.lastName}
                            name="lastName"
                        />
                    </Form.Group>
                    <Form.Group controlId="email">
                        <Form.Label className="pt-4">Email</Form.Label>
                        <Form.Control type="text" defaultValue={employee.email} name="email" />
                    </Form.Group>
                    <Form.Group controlId="password">
                        <Form.Label className="pt-4">Password</Form.Label>
                        <Form.Control
                            type="password"
                            defaultValue={employee.password}
                            name="password"
                        />
                    </Form.Group>
                    <ActionButtons cancel={RoutesNames.EMPLOYEES_LIST} action="Save changes" />
                </Form>
            </Container>
        </Container>
    );
}
