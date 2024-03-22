import { useEffect, useState } from "react";
import { Container, Table } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import TransactionService from "../../services/TransactionService";
import moment from "moment";

export default function Transactions() {
    const [transactions, setTransactions] = useState();
    let navigate = useNavigate();

    async function fetchTransactions() {
        await TransactionService.get()
            .then((res) => {
                setTransactions(res.data);
            })
            .catch((error) => {
                alert(error);
            });
    }

    useEffect(() => {
        fetchTransactions();
    }, []);

    function formatDate(transactionDate) {
        let mdp = moment.utc(transactionDate);
        if (mdp.hour() == 0 && mdp.minutes() == 0) {
            return mdp.format("DD. MM. YYYY.");
        }
        return mdp.format("DD. MM. YYYY. HH:mm");
    }

    return (
        <Container>
            <Table striped bordered hover responsive>
                <thead>
                    <tr>
                        <th>Transaction date</th>
                        <th>Status</th>
                    </tr>
                </thead>
                <tbody>
                    {transactions &&
                        transactions.map((inventoryTransaction, index) => (
                            <tr key={index}>
                                <td>
                                    {inventoryTransaction.transactionDate ==
                                    null
                                        ? 'Not defined'
                                        : formatDate(
                                              inventoryTransaction.transactionDate
                                          )}
                                </td>
                                <td>
                                    {inventoryTransaction.transactionStatusName}
                                </td>
                            </tr>
                        ))}
                </tbody>
            </Table>
        </Container>
    );
}
