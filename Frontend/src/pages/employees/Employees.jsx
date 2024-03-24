import { Button, Container, Table } from "react-bootstrap";
import { RoutesNames } from "../../constants";
import { useNavigate } from "react-router-dom";
import { FaEdit, FaTrash } from "react-icons/fa";
import SearchAndAdd from "../../components/SearchAndAdd";
import { useEffect, useState } from "react";
import EmployeeService from "../../services/EmployeeService";

export default function Employees() {
    const [employees, setEmployees] = useState();
    let navigate = useNavigate();

    async function fetchEmployees() {
        await EmployeeService.get()
            .then((res) => {
                setEmployees(res.data);
            })
            .catch((error) => {
                alert(error);
            });
    }

    useEffect(() => {
        fetchEmployees();
    }, []);

    async function removeEmployee(id) {
        const response = await EmployeeService.remove(id);

        if (response.ok) {
            alert(response.message.data.message);
            fetchEmployees();
        } else {
            alert(response.message);
        }
    }

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
