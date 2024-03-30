import React, { useEffect, useState } from "react";
import { Button, Col, Container, Form, Nav, Row, Table } from "react-bootstrap";
import { Link, useNavigate, useParams } from "react-router-dom";
import TransactionService from "../../services/TransactionService";
import EmployeeService from "../../services/EmployeeService";
import TransactionItemService from "../../services/TransactionItemService";
import moment from "moment";
import { RoutesNames } from "../../constants";
import TransactionOpen from "../../components/Transactions/TransactionOpen";
import TransactionClosed from "../../components/Transactions/TransactionClosed";
import WarehouseService from "../../services/WarehouseService";

export default function TransactionsEdit() {
    const navigate = useNavigate();
    const routeParams = useParams();
    const entityName = "transaction";

    const [transaction, setTransaction] = useState({});
    const [employees, setEmployees] = useState([]);
    const [employeeId, setEmployeeId] = useState(0);
    const [warehouses, setWarehouses] = useState([]);
    const [warehouseId, setWarehouseId] = useState(0);
    const [associatedWarehouses, setAssociatedWarehouses] = useState([]);
    const [associatedWarehouseId, setAssociatedWarehouseId] = useState(0);
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
        await fetchAssociatedWarehouses();
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
            alert(error.message);
        }
    }

    async function fetchWarehouses() {
        try {
            const response = await WarehouseService.get();
            const warehousesData = response.data;
            setWarehouses(warehousesData);
            setWarehouseId(warehousesData[0].id);
        } catch (error) {
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

    async function fetchAssociatedWarehouses() {
        try {
            const response = await TransactionItemService.GetWarehouses(routeParams.id);
            setAssociatedWarehouses(response.data);
        } catch (error) {
            alert(error.message);
        }
    }

    async function fetchProductsOnWarehouse(associatedWarehouseId) {
        try {
            const response = await TransactionItemService.GetProductsOnWarehouse(
                routeParams.id,
                associatedWarehouseId
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

    function handleTabChange(associatedWarehouseId) {
        setActiveTab(associatedWarehouseId);
        setSelectedWarehouseId(associatedWarehouseId);
        if (associatedWarehouseId === "all") {
            fetchProducts();
        } else {
            fetchProductsOnWarehouse(associatedWarehouseId);
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
                        <TransactionClosed
                            activeTab={activeTab}
                            handleTabChange={handleTabChange}
                            associatedWarehouses={associatedWarehouses}
                            products={products}
                            productsOnWarehouse={productsOnWarehouse}
                            isUnitary={isUnitary}
                        />
                    ) : (
                        <TransactionOpen
                            warehouses={warehouses}
                            warehouseId={selectedWarehouseId}
                            setWarehouseId={setSelectedWarehouseId}
                        />
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
