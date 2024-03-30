import React, { useEffect, useState } from "react";
import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { Link, useNavigate, useParams } from "react-router-dom";
import TransactionService from "../../services/TransactionService";
import EmployeeService from "../../services/EmployeeService";
import TransactionItemService from "../../services/TransactionItemService";
import moment from "moment";
import { RoutesNames } from "../../constants";
import TransactionOpen from "../../components/Transactions/TransactionOpen";
import TransactionClosed from "../../components/Transactions/TransactionClosed";
import WarehouseService from "../../services/WarehouseService";
import ProductService from "../../services/ProductService";
import TransactionDetailsForm from "../../components/Transactions/TransactionDetailsForm";

export default function TransactionsEdit() {
    const navigate = useNavigate();
    const routeParams = useParams();

    const [transaction, setTransaction] = useState({});

    const [employees, setEmployees] = useState([]);
    const [employeeId, setEmployeeId] = useState(0);

    const [warehouses, setWarehouses] = useState([]);
    const [associatedWarehouses, setAssociatedWarehouses] = useState([]);
    const [activeTab, setActiveTab] = useState("all");

    const [products, setProducts] = useState([]);
    const [associatedProducts, setAssociatedProducts] = useState([]);
    const [productsOnWarehouse, setProductsOnWarehouse] = useState([]);

    const [statusId, setStatusId] = useState(0);
    const [selectedWarehouseId, setSelectedWarehouseId] = useState(null);

    useEffect(() => {
        fetchInitialData();
    }, []);

    async function fetchInitialData() {
        await fetchEmployees();
        await fetchTransaction();
        await fetchAssociatedProducts();
        await fetchWarehouses();
        await fetchAssociatedWarehouses();
        await fetchProducts();
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
        } catch (error) {
            alert(error.message);
        }
    }

    async function fetchProducts() {
        try {
            const response = await ProductService.get();
            const productsData = response.data;
            setProducts(productsData);
        } catch (error) {
            alert(error.message);
        }
    }

    async function fetchAssociatedProducts() {
        try {
            const response = await TransactionItemService.GetProducts(routeParams.id);
            setAssociatedProducts(response.data);
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

    function handleTabWarehouseChange(associatedWarehouseId) {
        setActiveTab(associatedWarehouseId);
        setSelectedWarehouseId(associatedWarehouseId);
        if (associatedWarehouseId === "all") {
            fetchAssociatedProducts();
        } else {
            fetchProductsOnWarehouse(associatedWarehouseId);
        }
    }

    function handleSelectWarehouseChange(warehouseId) {
        if (warehouseId === "") {
            setSelectedWarehouseId(null);
        } else {
            setSelectedWarehouseId(warehouseId);
            fetchProductsOnWarehouse(warehouseId);
        }
    }

    return (
        <Container>
            <Form onSubmit={handleSubmit}>
                <Row>
                    <TransactionDetailsForm
                        transaction={transaction}
                        employees={employees}
                        employeeId={employeeId}
                        setEmployeeId={setEmployeeId}
                        additionalDetails={transaction.additionalDetails}
                        handleStatusToggle={handleStatusToggle}
                        statusId={statusId}
                        transactionStatus={transactionStatus}
                        transactionStatusName={transactionStatusName}
                    />
                    {statusId === 2 ? (
                        <TransactionClosed
                            activeTab={activeTab}
                            handleTabChange={handleTabWarehouseChange}
                            associatedWarehouses={associatedWarehouses}
                            products={associatedProducts}
                            productsOnWarehouse={productsOnWarehouse}
                            isUnitary={isUnitary}
                        />
                    ) : (
                        <TransactionOpen
                            warehouses={warehouses}
                            products={products}
                            warehouseId={selectedWarehouseId}
                            setWarehouseId={handleSelectWarehouseChange}
                            productsOnWarehouse={productsOnWarehouse}
                            associatedWarehouses={associatedWarehouses}
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
