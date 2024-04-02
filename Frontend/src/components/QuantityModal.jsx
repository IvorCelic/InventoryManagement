import React, { useState } from "react";
import { Form } from "react-bootstrap";
import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";

export default function ProductModal({ show, handleClose, handleAdd }) {
    const [quantity, setQuantity] = useState("");

    const handleChange = (event) => {
        setQuantity(event.target.value);
    };

    const handleConfirm = () => {
        if (quantity.trim() !== "") {
            handleAdd(parseInt(quantity));
            setQuantity("");
            handleClose();
        }
    };

    return (
        <Modal
            show={show}
            onHide={handleClose}
            backdrop={true}
            centered
            dialogClassName="custom-modal-dialog"
        >
            <Modal.Header closeButton>
                <Modal.Title>Insert Quantity</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form>
                    <Form.Group controlId="quantity">
                        <Form.Label>Quantity</Form.Label>
                        <Form.Control
                            type="text"
                            placeholder="Enter quantity"
                            value={quantity}
                            onChange={handleChange}
                        />
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={handleClose}>
                    Close
                </Button>
                <Button variant="primary" onClick={handleConfirm}>
                    Add
                </Button>
            </Modal.Footer>
        </Modal>
    );
}
