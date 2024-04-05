import React, { useEffect, useState } from "react";
import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { Link, useNavigate, useParams } from "react-router-dom";
import TransactionService from "../../services/TransactionService";
import EmployeeService from "../../services/EmployeeService";
import TransactionItemService from "../../services/TransactionItemService";
import { RoutesNames } from "../../constants";
import TransactionOpen from "./TransactionOpen";
import TransactionClosed from "./TransactionClosed";
import WarehouseService from "../../services/WarehouseService";
import ProductService from "../../services/ProductService";
import TransactionDetailsForm from "./TransactionEditForm";
import useError from "../../hooks/useError";
import ActionButtons from "../../components/ActionButtons";

export default function TransactionsEdit() {
    const navigate = useNavigate();
    const routeParams = useParams();

    const [transaction, setTransaction] = useState({});
    const [transactionId, setTransactionId] = useState(0);

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

    const { showError } = useError();

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
        const response = await TransactionService.edit(
            routeParams.id,
            entityName
        );
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
        const response = await TransactionService.getById(
            "InventoryTransaction",
            routeParams.id
        );
        if (!response.ok) {
            showError(response.data);
            return;
        }
        let transactionData = response.data;

        setTransaction(transactionData);
        setEmployeeId(transactionData.employeeId);
        setTransactionId(routeParams.id);
        setStatusId(transactionData.transactionStatusId);
    }

    async function fetchEmployees() {
        const response = await EmployeeService.get("Employee");
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setEmployees(response.data);
        setEmployeeId(response.data[0].id);
    }

    async function fetchWarehouses() {
        const response = await WarehouseService.get("Warehouse");
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setWarehouses(response.data);
    }

    async function fetchProducts() {
        const response = await ProductService.get("Product");
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setProducts(response.data);
    }

    async function fetchAssociatedProducts() {
        try {
            const response = await TransactionItemService.GetProducts(
                routeParams.id
            );
            setAssociatedProducts(response.data);
        } catch (error) {
            alert(error.message);
        }
    }

    async function fetchAssociatedWarehouses() {
        try {
            const response = await TransactionItemService.GetWarehouses(
                routeParams.id
            );
            setAssociatedWarehouses(response.data);
        } catch (error) {
            alert(error.message);
        }
    }

    async function fetchProductsOnWarehouse(associatedWarehouseId) {
        try {
            const response =
                await TransactionItemService.GetProductsOnWarehouse(
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
            return (
                <span style={{ color: "green" }}>
                    Transaction currently <strong>opened</strong>
                </span>
            );
        } else {
            return (
                <span style={{ color: "gray" }}>
                    Transaction currently <strong>closed</strong>
                </span>
            );
        }
    }

    function transactionStatusName(transactionStatusId) {
        return transactionStatusId === 1
            ? "Close transaction"
            : "Open transaction";
    }

    function handleStatusToggle() {
        setStatusId((previous) => (previous === 1 ? 2 : 1));
    }

    function ReadisUnitary(product) {
        if (product.isUnitary == null) return "Not defined";
        return product.isUnitary ? "Yes" : "No";
    }

    function isUnitary(product) {
        return product.isUnitary === 1 ? true : false;
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

    function handleProductOnoWarehouseChange() {
        fetchProductsOnWarehouse(selectedWarehouseId);
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
                            associatedProducts={associatedProducts}
                            productsOnWarehouse={productsOnWarehouse}
                            isUnitary={ReadisUnitary}
                        />
                    ) : (
                        <TransactionOpen
                            warehouses={warehouses}
                            products={products}
                            warehouseId={selectedWarehouseId}
                            setWarehouseId={handleSelectWarehouseChange}
                            productsOnWarehouse={productsOnWarehouse}
                            setProductsOnWarehouse={setProductsOnWarehouse}
                            associatedWarehouses={associatedWarehouses}
                            transactionId={transactionId}
                            handleProductOnoWarehouseChange={
                                handleProductOnoWarehouseChange
                            }
                            isUnitary={isUnitary}
                        />
                    )}
                </Row>
                <ActionButtons
                    cancel={RoutesNames.TRANSACTIONS_LIST}
                    action="Edit transaction"
                />
            </Form>
        </Container>
    );
}
