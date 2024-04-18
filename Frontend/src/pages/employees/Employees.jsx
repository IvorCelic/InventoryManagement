import { useEffect, useState } from "react";
import { Button, Card, Col, Container, Row, Table } from "react-bootstrap";
import EmployeeService from "../../services/EmployeeService";
import { RoutesNames } from "../../constants";
import { useNavigate } from "react-router-dom";
import { FaEdit, FaTrash } from "react-icons/fa";
import SearchAndAdd from "../../components/SearchAndAdd";
import useError from "../../hooks/useError";
import MyPagination from "../../components/MyPagination";
import useLoading from "../../hooks/useLoading";

export default function Employees() {
    const navigate = useNavigate();
    const { showError } = useError();
    const { showLoading, hideLoading } = useLoading();

    const [employees, setEmployees] = useState();
    const [totalEmployees, setTotalEmployees] = useState();
    const [page, setPage] = useState(1);
    const [condition, setCondition] = useState("");

    async function fetchEmployees() {
        showLoading();
        const responsePagination = await EmployeeService.getPagination(
            page,
            condition
        );
        const responseEmployee = await EmployeeService.get("Employee");
        if (!responsePagination.ok) {
            showError(response.data);
            return;
        }
        if (responsePagination.data.length == 0) {
            setPage(page - 1);
            return;
        }
        setEmployees(responsePagination.data);
        setTotalEmployees(responseEmployee.data.length);
        hideLoading();
    }

    useEffect(() => {
        fetchEmployees();
    }, [page, condition]);

    async function removeEmployee(id) {
        showLoading();
        const response = await EmployeeService.remove(`Employee`, id);
        showError(response.data);
        if (response.ok) {
            fetchEmployees();
        }
        hideLoading();
    }

    const totalPages = Math.ceil(totalEmployees / 8);

    function handlePageChange(page) {
        setPage(page);
    }

    function handleSearch(searchTerm) {
        setPage(1);
        setCondition(searchTerm);
    }

    return (
        <Container>
            <Col>
                <Row>
                    <SearchAndAdd
                        RouteName={RoutesNames.EMPLOYEES_CREATE}
                        entity={"employee"}
                        onSearch={handleSearch}
                    />
                </Row>
                <Row>
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
                                                        navigate(
                                                            `/employees/${employee.id}`
                                                        );
                                                    }}
                                                >
                                                    <FaEdit size={25} />
                                                </Button>
                                                <Button
                                                    variant="link"
                                                    className="link-danger actionButton"
                                                    onClick={() =>
                                                        removeEmployee(
                                                            employee.id
                                                        )
                                                    }
                                                >
                                                    <FaTrash size={25} />
                                                </Button>
                                            </Container>
                                        </td>
                                    </tr>
                                ))}
                        </tbody>
                    </Table>
                </Row>
                <Row>
                    <Col className="d-flex justify-content-center">
                        <MyPagination
                            currentPage={page}
                            totalPages={totalPages}
                            onPageChange={handlePageChange}
                        />
                    </Col>
                </Row>
                <Row className="d-flex justify-content-center">
                    {employees &&
                        employees.map((employee, index) => (
                            <Card
                                style={{ width: "16.6rem", margin: "0.5rem" }}
                            >
                                <Card.Img
                                    variant="top"
                                    src="holder.js/100px180"
                                />
                                <Card.Body
                                    key={index}
                                    className="d-flex flex-column align-items-center"
                                >
                                    <Card.Title>
                                        {employee.firstName} {employee.lastName}
                                    </Card.Title>
                                    <Card.Text>{employee.email}</Card.Text>
                                    <div className="d-flex justify-content-center">
                                        <Button
                                            variant="link"
                                            className="me-2 "
                                            onClick={() => {
                                                navigate(
                                                    `/employees/${employee.id}`
                                                );
                                            }}
                                        >
                                            <FaEdit size={25} />
                                        </Button>
                                        <Button
                                            variant="link"
                                            className="link-danger"
                                            onClick={() =>
                                                removeEmployee(employee.id)
                                            }
                                        >
                                            <FaTrash size={25} />
                                        </Button>
                                    </div>
                                </Card.Body>
                            </Card>
                        ))}
                </Row>
            </Col>
        </Container>
    );
}
