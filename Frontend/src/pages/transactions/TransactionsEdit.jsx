import React, { useEffect, useState } from "react";
import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { Link, useNavigate, useParams } from "react-router-dom";
import TransactionService from "../../services/TransactionService";
import EmployeeService from "../../services/EmployeeService";
import TransactionItemService from "../../services/TransactionItemService";
import { RoutesNames } from "../../constants";

import TransactionClosed from "./transactionItems/TransactionClosed";
import TransactionOpen from "./transactionItems/TransactionOpen";
import TransactionDetailsForm from "./TransactionEditForm";
import useError from "../../hooks/useError";
import ActionButtons from "../../components/ActionButtons";
import useLoading from "../../hooks/useLoading";

export default function TransactionsEdit() {
    const navigate = useNavigate();
    const routeParams = useParams();

    const [transaction, setTransaction] = useState({});
    const [transactionId, setTransactionId] = useState(0);

    const [employees, setEmployees] = useState([]);
    const [employeeId, setEmployeeId] = useState(0);

    // const [warehouses, setWarehouses] = useState([]);
    // const [associatedWarehouses, setAssociatedWarehouses] = useState([]);
    // const [activeTab, setActiveTab] = useState("all");

    // const [products, setProducts] = useState([]);
    // const [associatedProducts, setAssociatedProducts] = useState([]);
    // const [productsOnWarehouse, setProductsOnWarehouse] = useState([]);

    const [statusId, setStatusId] = useState(0);
    // const [selectedWarehouseId, setSelectedWarehouseId] = useState(null);

    const { showError } = useError();
    const { showLoading, hideLoading } = useLoading();

    async function fetchTransaction() {
        showLoading();
        const response = await TransactionService.getById("InventoryTransaction", routeParams.id);
        if (!response.ok) {
            showError(response.data);
            return;
        }
        let transactionData = response.data;

        setTransaction(transactionData);
        setEmployeeId(transactionData.employeeId);
        setTransactionId(routeParams.id);
        setStatusId(transactionData.transactionStatusId);
        hideLoading();
    }

    async function fetchEmployees() {
        showLoading();
        const response = await EmployeeService.get("Employee");
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setEmployees(response.data);
        setEmployeeId(response.data[0].id);
        hideLoading();
    }

    useEffect(() => {
        fetchInitialData();
    }, []);

    async function fetchInitialData() {
        await fetchEmployees();
        await fetchTransaction();
        // await fetchWarehouses();
        // await fetchProducts();
    }

    async function edit(entityName) {
        showLoading();
        const response = await TransactionService.edit(
            "InventoryTransaction",
            routeParams.id,
            entityName
        );
        if (response.ok) {
            navigate(RoutesNames.TRANSACTIONS_LIST);
            return;
        }
        showError(response.data);
        hideLoading();
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

    // async function fetchWarehouses() {
    //     const response = await WarehouseService.get("Warehouse");
    //     if (!response.ok) {
    //         showError(response.data);
    //         return;
    //     }
    //     setWarehouses(response.data);
    // }

    // async function fetchProducts() {
    //     const response = await ProductService.get("Product");
    //     if (!response.ok) {
    //         showError(response.data);
    //         return;
    //     }
    //     setProducts(response.data);
    // }

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
        return transactionStatusId === 1 ? "Close transaction" : "Open transaction";
    }

    function handleStatusToggle() {
        setStatusId((previous) => (previous === 1 ? 2 : 1));
    }

    // function ReadisUnitary(product) {
    //     if (product.isUnitary == null) return "Not defined";
    //     return product.isUnitary ? "Yes" : "No";
    // }

    // function isUnitary(product) {
    //     return product.isUnitary === 1 ? true : false;
    // }

    // function handleTabWarehouseChange(associatedWarehouseId) {
    //     setActiveTab(associatedWarehouseId);
    //     setSelectedWarehouseId(associatedWarehouseId);
    //     if (associatedWarehouseId === "all") {
    //         fetchAssociatedProducts();
    //     } else {
    //         fetchProductsOnWarehouse(associatedWarehouseId);
    //     }
    // }

    // function handleSelectWarehouseChange(warehouseId) {
    //     if (warehouseId === "") {
    //         setSelectedWarehouseId(null);
    //     } else {
    //         setSelectedWarehouseId(warehouseId);
    //         fetchProductsOnWarehouse(warehouseId);
    //     }
    // }

    // function handleProductOnoWarehouseChange() {
    //     fetchProductsOnWarehouse(selectedWarehouseId);
    // }

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
                    {statusId === 2 ? <TransactionClosed /> : <TransactionOpen />}
                </Row>
                <ActionButtons cancel={RoutesNames.TRANSACTIONS_LIST} action="Edit transaction" />
            </Form>
        </Container>
    );
}
