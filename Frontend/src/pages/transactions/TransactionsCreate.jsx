import { useEffect, useState } from "react";
import { Button, Col, Container, Form, Row, Table } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import moment from "moment";
import { RoutesNames } from "../../constants";
import EmployeeService from "../../services/EmployeeService";
import TransactionService from "../../services/TransactionService";

export default function TransactionsCreate() {
    const navigate = useNavigate();
    const entityName = "Transaction";

    const [employees, setEmployees] = useState([]);
    const [employeeId, setEmployeeId] = useState(0);

    async function fetchEmployees() {
        await EmployeeService.get().then((res) => {
            setEmployees(res.data);
            setEmployeeId(res.data[0].id);
        });
    }

    async function load() {
        await fetchEmployees();
    }

    useEffect(() => {
        load();
    }, []);

    async function add(entity) {
        entity.transactionDate = moment().toISOString(); // sets the current date

        const response = await TransactionService.add(entity);
        if (response.ok) {
            navigate(RoutesNames.TRANSACTIONS_LIST);
        } else {
            alert(response.message.errors);
        }
    }

    function handleSubmit(entity) {
        entity.preventDefault();

        const data = new FormData(entity.target);

        add({
            employeeId: parseInt(employeeId),
            transactionStatusId: 1,
            additionalDetails: data.get("additionaldetails"),
        });
    }

    return (
        <Container className="square border mt-5">
            <h2 className="mt-5 ms-5">Add new {entityName}</h2>
            <Form className="m-5" onSubmit={handleSubmit}>
                <Form.Group controlId="employee">
                    <Form.Label>Employee</Form.Label>
                    <Form.Select
                        onChange={(entity) => {
                            setEmployeeId(entity.target.value);
                        }}
                    >
                        {employees.map((employee, index) => (
                            <option key={index} value={employee.id}>
                                {employee.firstName} {employee.lastName}
                            </option>
                        ))}
                    </Form.Select>
                </Form.Group>

                <Form.Group controlId="additionaldetails">
                    <Form.Label className="pt-4">Additional Details</Form.Label>
                    <Form.Control
                        placeholder="Additional details"
                        type="text"
                        name="additionaldetails"
                    />
                </Form.Group>

                <Row className="mb-0 flex-column flex-sm-row">
                    <Col className="d-flex align-items-center mb-2 mb-sm-0">
                        <Link
                            className="btn btn-danger myButton"
                            to={RoutesNames.TRANSACTIONS_LIST}
                        >
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
    );
}
