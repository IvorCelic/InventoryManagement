import { useEffect, useState } from "react";
import { Button, Card, Col, Container, Row, Image } from "react-bootstrap";
import EmployeeService from "../../services/EmployeeService";
import { App, RoutesNames } from "../../constants";
import { useNavigate } from "react-router-dom";
import { FaEdit, FaTrash } from "react-icons/fa";
import SearchAndAdd from "../../components/SearchAndAdd";
import useError from "../../hooks/useError";
import MyPagination from "../../components/MyPagination";
import useLoading from "../../hooks/useLoading";
import unknown from "../../assets/unknown.png";

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
        const responsePagination = await EmployeeService.GetPagination(
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

    function image(employee) {
        if (employee.image != null) {
            return App.URL + employee.image + `?${Date.now()}`;
        }
        return unknown;
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
                <Row className="d-flex justify-content-center">
                    {employees &&
                        employees.map((employee, index) => (
                            <Card
                                key={employee.id}
                                className="align-items-center card-container"
                            >
                                <div className="card-image-container">
                                    <Image
                                        variant="top"
                                        src={image(employee)}
                                        className="image mt-3"
                                        roundedCircle
                                    />
                                </div>
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
                <Row className="mt-5">
                    <Col className="d-flex justify-content-center">
                        <MyPagination
                            currentPage={page}
                            totalPages={totalPages}
                            onPageChange={handlePageChange}
                        />
                    </Col>
                </Row>
            </Col>
        </Container>
    );
}
