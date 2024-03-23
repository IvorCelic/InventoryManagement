import { useEffect, useState } from "react";
import { Button, Container, Table } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import TransactionService from "../../services/TransactionService";
import moment from "moment";
import { FaEdit, FaTrash } from "react-icons/fa";

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
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody>
                    {transactions &&
                        transactions.map((inventoryTransaction, index) => (
                            <Link>
                                <tr key={index}>
                                    <td>
                                        {inventoryTransaction.transactionDate == null
                                            ? "Not defined"
                                            : formatDate(inventoryTransaction.transactionDate)}
                                    </td>
                                    <td>{inventoryTransaction.transactionStatusName}</td>
                                    <td>
                                        {" "}
                                        <Container className="d-flex justify-content-center">
                                            <Button
                                                variant="link"
                                                className="me-2 actionButton"
                                                // onClick={() => {
                                                //     navigate(`/products/${product.id}`);
                                                // }}
                                            >
                                                <FaEdit size={25} />
                                            </Button>
                                            <Button
                                                className="link-danger actionButton"
                                                // onClick={() => removeProduct(product.id)}
                                            >
                                                <FaTrash size={25} />
                                            </Button>
                                        </Container>
                                    </td>
                                </tr>
                            </Link>
                        ))}
                </tbody>
            </Table>
        </Container>
    );
}
