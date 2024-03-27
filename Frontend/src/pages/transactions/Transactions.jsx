import { useEffect, useState } from "react";
import { Button, Container, Table } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import TransactionService from "../../services/TransactionService";
import moment from "moment";
import { FaEdit, FaFilePdf, FaPrint, FaTrash } from "react-icons/fa";
import SearchAndAdd from "../../components/SearchAndAdd";
import { RoutesNames } from "../../constants";

export default function Transactions() {
    const [transactions, setTransactions] = useState();
    const navigate = useNavigate();

    async function fetchTransactions() {
        await TransactionService.get()
            .then((res) => {
                console.log(res.data);
                setTransactions(res.data);
            })
            .catch((error) => {
                alert(error);
            });
    }

    useEffect(() => {
        fetchTransactions();
    }, []);

    async function removeTransaction(id) {
        const response = await TransactionService.remove(id);
        if (response.ok) {
            alert(response.message.data.message);
            fetchTransactions();
        }
    }

    function formatDate(transactionDate) {
        let mdp = moment.utc(transactionDate);
        if (mdp.hour() == 0 && mdp.minutes() == 0) {
            return mdp.format("DD. MM. YYYY.");
        }
        return mdp.format("DD. MM. YYYY. HH:mm");
    }

    return (
        <Container>
            <Container>
                <SearchAndAdd RouteName={RoutesNames.TRANSACTIONS_CREATE} entity={"transaction"} />
            </Container>
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
                                <td>{inventoryTransaction.employeeFirstLastName}</td>
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
                                            <FaFilePdf size={25} />
                                        </Button>
                                        <Button variant="link">
                                            <FaPrint size={25} />
                                        </Button>
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
        </Container>
    );
}
