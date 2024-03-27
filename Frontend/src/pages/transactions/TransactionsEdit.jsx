import { useEffect, useState } from "react";
import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { Link, useNavigate, useParams } from "react-router-dom";
import TransactionService from "../../services/TransactionService";
import EmployeeService from "../../services/EmployeeService";
import moment from "moment";
import { RoutesNames } from "../../constants";

export default function TransactionsEdit() {
    const navigate = useNavigate();
    const routeParams = useParams();
    const entityName = "Transaction";

    const [transaction, setTransaction] = useState({});

    const [employees, setEmployees] = useState([]);
    const [employeeId, setEmployeeId] = useState(0);

    async function fetchInitialData() {
        await fetchEmployees();
        await fetchTransaction();
    }

    useEffect(() => {
        fetchInitialData();
    }, []);

    async function edit(entity) {
        const response = await TransactionService.edit(routeParams.id, entity);
        if (response.ok) {
            navigate(RoutesNames.TRANSACTIONS_LIST);
        } else {
            alert(response.message.error);
        }
    }

    function handleSubmit(entity) {
        error.preventDefault();

        const data = new FormData(entity.target);

        edit({
            employeeId: parseInt(employeeId),
            transactionStatusId: transactionStatusId === 1 ? 2 : 1,
            additionalDetails: data.get("additionaldetails"),
        });
    }

    async function fetchTransaction() {
        await TransactionService.getById(routeParams.id)
            .then((res) => {
                let transaction = res.data;

                setEmployeeId(transaction.employeeId);

                setTransaction(transaction);
            })
            .catch((error) => {
                alert(error.message);
            });
    }

    async function fetchEmployees() {
        await EmployeeService.get()
            .then((res) => {
                setEmployees(res.data);
                setEmployeeId(res.data[0].id);
            })
            .catch((error) => {
                alert(error.message);
            });
    }

    function formatDate(transactionDate) {
        let mdp = moment.utc(transactionDate);
        if (mdp.hour() == 0 && mdp.minutes() == 0) {
            return mdp.format("DD. MM. YYYY.");
        }
        return mdp.format("DD. MM. YYYY. HH:mm");
    }

    function transactionStatus(transactionStatusId) {
        if (transactionStatusId === 1) {
            return "Transaction currently opened";
        } else if (transactionStatusId === 2) {
            return "Transaction currently closed";
        } else {
            return "Transaction invalid";
        }
    }

    function transactionStatusName(transactionStatusId) {
        return transactionStatusId === 1 ? "Close transaction" : "Open transaction";
    }

    return (
        <Container>
            <Form onSubmit={handleSubmit}>
                <Row>
                    <Col lg={4} md={12} sm={12} className="mt-5 transactionEditContainer">
                        <h3>Transaction date</h3>
                        <p>{formatDate(transaction.transactionDate)}</p>
                        <Form.Group className="mb-3 pt-4" controlId="employee">
                            <Form.Label>Employee</Form.Label>
                            <Form.Select
                                value={employeeId}
                                onChange={(entity) => {
                                    setEmployeeId(entity.target.value);
                                }}
                            >
                                {employees &&
                                    employees.map((employee, index) => (
                                        <option key={index} value={employee.id}>
                                            {employee.firstName} {employee.lastName}
                                        </option>
                                    ))}
                            </Form.Select>
                        </Form.Group>
                        <Form.Group controlId="additionaldetails" className="pt-2">
                            <Form.Label>Additional Details</Form.Label>
                            <Form.Control
                                type="text"
                                name="additionaldetails"
                                defaultValue={transaction.additionalDetails}
                                maxLength={255}
                                required
                            />
                        </Form.Group>
                        <p className="pt-5">{transactionStatus(transaction.transactionStatusId)}</p>
                        <Button variant="secondary" className="btn buttonStatus">
                            {transactionStatusName(transaction.transactionStatusId)}
                        </Button>
                    </Col>
                    {transaction.transactionStatusId === 1 ? (
                        <Col
                            lg={8}
                            md={12}
                            sm={12}
                            className="square border mt-5 transactionEditContainer"
                        >
                            TEST
                        </Col>
                    ) : null}
                </Row>
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
