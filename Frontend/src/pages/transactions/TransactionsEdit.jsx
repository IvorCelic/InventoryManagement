import React, { useEffect, useState } from "react";
import { Button, Col, Container, Form, Nav, Row, Table } from "react-bootstrap";
import { Link, useNavigate, useParams } from "react-router-dom";
import TransactionService from "../../services/TransactionService";
import EmployeeService from "../../services/EmployeeService";
import TransactionItemService from "../../services/TransactionItemService";
import moment from "moment";
import { RoutesNames } from "../../constants";

export default function TransactionsEdit() {
    const navigate = useNavigate();
    const routeParams = useParams();
    const entityName = "transaction";

    const [transaction, setTransaction] = useState({});
    const [employees, setEmployees] = useState([]);
    const [employeeId, setEmployeeId] = useState(0);
    const [warehouses, setWarehouses] = useState([]);
    const [products, setProducts] = useState([]);
    const [statusId, setStatusId] = useState(0);
    const [activeTab, setActiveTab] = useState("all");
    const [productsOnWarehouse, setProductsOnWarehouse] = useState([]);
    const [selectedWarehouseId, setSelectedWarehouseId] = useState(null);

    useEffect(() => {
        fetchInitialData();
    }, []);

    async function fetchInitialData() {
        await fetchEmployees();
        await fetchTransaction();
        await fetchProducts();
        await fetchWarehouses();
    }

    async function edit(entityName) {
        const response = await TransactionService.edit(routeParams.id, entityName);
        if (response.ok) {
            navigate(RoutesNames.TRANSACTIONS_LIST);
        } else {
            alert(response.message.error);
        }
    }

    function handleSubmit(event) {
        event.preventDefault();

        const data = new FormData(event.target);

        edit({
            employeeId: parseInt(employeeId),
            transactionStatusId: parseInt(statusId),
            additionalDetails: data.get("additionaldetails"),
        });
    }

    async function fetchTransaction() {
        try {
            const response = await TransactionService.getById(routeParams.id);
            const transactionData = response.data;
            setEmployeeId(transactionData.employeeId);
            setTransaction(transactionData);
            setStatusId(transactionData.transactionStatusId);
        } catch (error) {
            alert(error.message);
        }
    }

    async function fetchEmployees() {
        try {
            const response = await EmployeeService.get();
            const employeesData = response.data;
            setEmployees(employeesData);
            setEmployeeId(employeesData[0].id);
        } catch (error) {
            console.error("Error fetching employees:", error);
            alert(error.message);
        }
    }

    async function fetchProducts() {
        try {
            const response = await TransactionItemService.GetProducts(routeParams.id);
            setProducts(response.data);
        } catch (error) {
            alert(error.message);
        }
    }

    async function fetchWarehouses() {
        try {
            const response = await TransactionItemService.GetWarehouses(routeParams.id);
            setWarehouses(response.data);
        } catch (error) {
            alert(error.message);
        }
    }

    async function fetchProductsOnWarehouse(warehouseId) {
        try {
            const response = await TransactionItemService.GetProductsOnWarehouse(
                routeParams.id,
                warehouseId
            );
            setProductsOnWarehouse(response.data);
        } catch (error) {
            alert(error.message);
        }
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

    function handleStatusToggle() {
        setStatusId((previous) => (previous === 1 ? 2 : 1));
    }

    function isUnitary(product) {
        if (product.isUnitary == null) return "Not defined";
        return product.isUnitary ? "Yes" : "No";
    }

    function handleTabChange(warehouseId) {
        setActiveTab(warehouseId);
        setSelectedWarehouseId(warehouseId);
        if (warehouseId === "all") {
            fetchProducts();
        } else {
            fetchProductsOnWarehouse(warehouseId);
        }
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
                                onChange={(e) => setEmployeeId(e.target.value)}
                            >
                                {employees.map((employee) => (
                                    <option key={employee.id} value={employee.id}>
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
                        <p className="pt-5">{transactionStatus(statusId)}</p>
                        <Button
                            variant="secondary"
                            className="btn buttonStatus"
                            onClick={handleStatusToggle}
                        >
                            {transactionStatusName(statusId)}
                        </Button>
                    </Col>
                    {statusId === 2 ? (
                        <Col lg={8} md={12} sm={12} className="mt-5 transactionEditContainer">
                            <Row className="horizontal-tabs-container">
                                <Nav className="horizontal-tabs">
                                    <Nav.Item>
                                        <Nav.Link
                                            eventKey="all"
                                            active={activeTab === "all"}
                                            onClick={() => handleTabChange("all")}
                                        >
                                            All
                                        </Nav.Link>
                                    </Nav.Item>
                                    {warehouses.map((warehouse) => (
                                        <Nav.Item key={warehouse.id}>
                                            <Nav.Link
                                                eventKey={warehouse.id}
                                                active={activeTab === warehouse.id}
                                                onClick={() => handleTabChange(warehouse.id)}
                                            >
                                                {warehouse.warehouseName}
                                            </Nav.Link>
                                        </Nav.Item>
                                    ))}
                                </Nav>
                            </Row>
                            <Row className="mt-3">
                                <Col>
                                    <h5 className="mt-3">Products</h5>
                                    <Table striped bordered hover size="sm" className="table-sm">
                                        <thead>
                                            <tr>
                                                <th>Name</th>
                                                <th>Is Unitary</th>
                                                <th>Quantity</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {activeTab === "all"
                                                ? products.map((product, index) => (
                                                      <tr key={index}>
                                                          <td>{product.productName}</td>
                                                          <td>{isUnitary(product)}</td>
                                                          <td>Quantity</td>
                                                      </tr>
                                                  ))
                                                : productsOnWarehouse.map((product, index) => (
                                                      <tr key={index}>
                                                          <td>{product.productName}</td>
                                                          <td>{isUnitary(product)}</td>
                                                          <td>Quantity</td>
                                                      </tr>
                                                  ))}
                                        </tbody>
                                    </Table>
                                </Col>
                            </Row>
                        </Col>
                    ) : (
                        <Col
                            lg={8}
                            md={12}
                            sm={12}
                            className="border mt-5 transactionEditContainer"
                        >
                            TEST
                        </Col>
                    )}
                </Row>
                <Row className="mb-0 flex-column flex-sm-row">
                    <Col>
                        <Link
                            className="btn btn-danger myButton"
                            to={RoutesNames.TRANSACTIONS_LIST}
                        >
                            Cancel
                        </Link>
                    </Col>
                    <Col>
                        <Button className="myButton" variant="primary" type="submit">
                            Save changes
                        </Button>
                    </Col>
                </Row>
            </Form>
        </Container>
    );
}
