import { Button, Col, Container, Row, Table } from "react-bootstrap";
import { RoutesNames } from "../../constants";
import SearchAndAdd from "../../components/SearchAndAdd";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import TransactionService from "../../services/TransactionService";
import { FaEdit, FaFilePdf, FaPrint, FaTrash } from "react-icons/fa";
import moment from "moment";
import useError from "../../hooks/useError";
import useLoading from "../../hooks/useLoading";

export default function Transactions() {
    const [transactions, setTransactions] = useState();
    let navigate = useNavigate();
    const { showError } = useError();
    const { showLoading, hideLoading } = useLoading();

    async function fetchTransactions() {
        showLoading();
        const response = await TransactionService.get("InventoryTransaction");
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setTransactions(response.data);
        hideLoading();
    }

    async function getTransactionReport(id) {
        showLoading();
        const response = await TransactionService.GetTransactionReport("InventoryTransaction", id);
        if (!response.ok) {
            showError(response.data);
            hideLoading();
            return;
        }
        
        const blob = new Blob([response.data], { type: "application/pdf" });

        saveAs(blob, `test_${id}.pdf`);

        hideLoading();
    }

    useEffect(() => {
        fetchTransactions();
    }, []);

    async function removeTransaction(id) {
        showLoading();
        const response = await TransactionService.remove("InventoryTransaction", id);
        showError(response.data);
        if (response.ok) {
            fetchTransactions();
        }
        hideLoading();
    }

    function formatDate(transactionDate) {
        let mdp = moment.utc(transactionDate);

        return mdp.format("DD. MM. YYYY.");
    }

    return (
        <Container>
            <Col>
                <Row>
                    <SearchAndAdd
                        RouteName={RoutesNames.TRANSACTIONS_CREATE}
                        entity={"transaction"}
                    />
                </Row>
                <Row>
                    <Table striped bordered hover responsive>
                        <thead>
                            <tr>
                                <th>Date</th>
                                <th>Details</th>
                                <th>Employee</th>
                                <th>Status</th>
                                <th>Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            {transactions &&
                                transactions.map((inventoryTransaction, index) => (
                                    <tr key={index}>
                                        <td>
                                            {inventoryTransaction.transactionDate == null
                                                ? "Not defined"
                                                : formatDate(inventoryTransaction.transactionDate)}
                                        </td>
                                        <td>{inventoryTransaction.additionalDetails}</td>
                                        <td>{inventoryTransaction.employeeName}</td>
                                        <td>{inventoryTransaction.transactionStatusName}</td>
                                        <td>
                                            <Container className="d-flex justify-content-center">
                                                <Button
                                                    variant="link"
                                                    onClick={() => {
                                                        navigate(
                                                            `/transactions/${inventoryTransaction.id}`
                                                        );
                                                    }}
                                                >
                                                    <FaEdit size={25} />
                                                </Button>
                                                <Button variant="link">
                                                    <FaFilePdf
                                                        size={25}
                                                        title="Generate PDF report"
                                                        onClick={() =>
                                                            getTransactionReport(
                                                                inventoryTransaction.id
                                                            )
                                                        }
                                                    />
                                                </Button>
                                                {/* <Button variant="link">
                                                    <FaPrint size={25} />
                                                </Button> */}
                                                <Button
                                                    variant="link"
                                                    className="link-danger"
                                                    onClick={() =>
                                                        removeTransaction(inventoryTransaction.id)
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
            </Col>
        </Container>
    );
}
