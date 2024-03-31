import React from "react";
import { Col, Form, Button } from "react-bootstrap";
import moment from "moment";

export default function TransactionDetailsForm({
    transaction,
    employees,
    employeeId,
    setEmployeeId,
    handleStatusToggle,
    statusId,
    transactionStatus,
    transactionStatusName,
}) {
    function formatDate(transactionDate) {
        let mdp = moment.utc(transactionDate);
        if (mdp.hour() == 0 && mdp.minutes() == 0) {
            return mdp.format("DD. MM. YYYY.");
        }
        return mdp.format("DD. MM. YYYY. HH:mm");
    }

    return (
        <Col lg={4} md={12} sm={12} className="mt-5 transactionEditContainer">
            <h3>Transaction date</h3>
            <p>{formatDate(transaction.transactionDate)}</p>
            <Form.Group className="mb-3 pt-4" controlId="employee">
                <Form.Label>Employee</Form.Label>
                <Form.Select value={employeeId} onChange={(e) => setEmployeeId(e.target.value)}>
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
            <Button variant="secondary" className="btn buttonStatus" onClick={handleStatusToggle}>
                {transactionStatusName(statusId)}
            </Button>
        </Col>
    );
}
