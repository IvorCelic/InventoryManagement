import { useEffect, useState } from "react";
import { Button, Container, Table } from "react-bootstrap";
import EmployeeService from "../../services/EmployeeService";
import { RoutesNames } from "../../constants";
import { useNavigate } from "react-router-dom";
import { FaEdit, FaTrash } from "react-icons/fa";
import SearchAndAdd from "../../components/SearchAndAdd";
import useError from "../../hooks/useError";

export default function Employees() {
    const [employees, setEmployees] = useState();
    let navigate = useNavigate();
    const { showError } = useError();

    async function fetchEmployees() {
        const response = await EmployeeService.get(`Employee`);
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setEmployees(response.data);
    }

    async function removeEmployee(id) {
        const response = await EmployeeService.remove(`Employee`, id);
        showError(response.data);
        if (response.ok) {
            fetchEmployees();
        }
    }

    useEffect(() => {
        fetchEmployees();
    }, []);

    return (
        <Container>
            <Container>
                <SearchAndAdd RouteName={RoutesNames.EMPLOYEES_CREATE} entity={"employee"} />
            </Container>
            <Table striped bordered hover responsive>
                <thead>
                    <tr>
                        <th>First name</th>
                        <th>Last name</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    {employees &&
                        employees.map((employee, index) => (
                            <tr key={index}>
                                <td>{employee.firstName}</td>
                                <td>{employee.lastName}</td>
                                <td>
                                    <Container className="d-flex justify-content-center">
                                        <Button
                                            variant="link"
                                            className="me-2 actionButton"
                                            onClick={() => {
                                                navigate(`/employees/${employee.id}`);
                                            }}
                                        >
                                            <FaEdit size={25} />
                                        </Button>
                                        <Button
                                            variant="link"
                                            className="link-danger actionButton"
                                            onClick={() => removeEmployee(employee.id)}
                                        >
                                            <FaTrash size={25} />
                                        </Button>
                                    </Container>
                                </td>
                            </tr>
                        ))}
                </tbody>
            </Table>
        </Container>
    );
}
