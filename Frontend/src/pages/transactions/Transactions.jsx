import { useEffect, useState } from "react";
import { Button, Card, Container, ListGroup, ListGroupItem, Table } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import TransactionService from "../../services/TransactionService";
import moment from "moment";
import { FaFilePdf, FaPrint, FaTrash } from "react-icons/fa";
import { RoutesNames } from "../../constants";

export default function Transactions() {
    const [transactions, setTransactions] = useState();
    const [transaction, setTransaction] = useState({});
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

    function transactionOpen(transactionStatusName) {
        if (transactionStatusName == "Transaction opened") {
            return "Close transaction";
        }

        return "Open transaction";
    }

    async function removeTransaction(id) {
        const response = await TransactionService.remove(id);
        if (response.ok) {
            alert(response.message.data.message);
            fetchTransactions();
        }
    }

    async function changeTransactionStatus(entityName) {
        const response = await TransactionService.edit(routeParams.id, entityName);
        if (response.ok) {
            navigate(RoutesNames.TRANSACTIONS_LIST);
        } else {
            console.log(response);
            alert(response.message);
        }
    }

    return (
        <Container>
            <ListGroup>
                {transactions &&
                    transactions.map((inventoryTransaction, index) => (
                        <ListGroup.Item action key={index} onClick={() => (RoutesNames.TRANSACTIONITEMS_LIST)}>
                            <div className="d-flex justify-content-between align-items-center">
                                <div className="d-flex flex-column">
                                    <p className="mb-0">
                                        {inventoryTransaction.transactionDate == null
                                            ? "Not defined"
                                            : formatDate(inventoryTransaction.transactionDate)}
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        {inventoryTransaction.transactionStatusName}
                                    </p>
                                </div>
                                <div>
                                    <Button
                                        variant="link"
                                        onClick={() => {
                                            const newStatusId =
                                                inventoryTransaction.transactionStatus;
                                            changeTransactionStatus(inventoryTransaction);
                                        }}
                                    >
                                        {transactionOpen(
                                            inventoryTransaction.transactionStatusName
                                        )}
                                    </Button>
                                    <Button variant="link" className="ml-2">
                                        <FaFilePdf size={25} />
                                    </Button>
                                    <Button variant="link" className="ml-2">
                                        <FaPrint size={25} />
                                    </Button>
                                    <Button
                                        variant="link"
                                        className="ml-2 link-danger"
                                        onClick={() => removeTransaction(inventoryTransaction.id)}
                                    >
                                        <FaTrash size={25} />
                                    </Button>
                                </div>
                            </div>
                        </ListGroup.Item>
                    ))}
            </ListGroup>
        </Container>
    );
}
