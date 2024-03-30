import React from "react";
import { Col, Form, Row } from "react-bootstrap";
import { FaCirclePlus } from "react-icons/fa6";
import { FaMinusCircle } from "react-icons/fa";

export default function TransactionOpen({ warehouses, warehouseId, setWarehouseId }) {
    function nameWarehouse() {
        for (let i = 0; i < warehouses.length; i++) {
            const entity = warehouses[i];
            if (entity.id == warehouseId) {
                return entity.warehouseName;
            }
        }
    }

    return (
        <Col lg={8} md={12} sm={12} className="border mt-5 transactionEditContainer">
            <Row className="align-items-center">
                <Col>
                    <Form.Group className="mb-3 pt-2 ms-2 me-3" controlId="warehouse">
                        <Form.Label>Warehouse</Form.Label>
                        <Form.Select
                            value={warehouseId}
                            onChange={(e) => setWarehouseId(e.target.value)}
                        >
                            <option value="">Select warehouse</option>
                            {warehouses &&
                                warehouses.map((warehouse) => (
                                    <option key={warehouse.id} value={warehouse.id}>
                                        {warehouse.warehouseName}
                                    </option>
                                ))}
                        </Form.Select>
                    </Form.Group>
                </Col>
                <Col className="warehouse-name">
                    <h2 className="pt-4">{nameWarehouse()}</h2>
                </Col>
            </Row>
            <Row className="mt-4">
                <Col className="border me-4 ms-4">
                    <h4 className="mb-3 mt-2 ms-2">Products</h4>
                    <ul className="product-list ms-2 me-2">
                        <li>
                            <span className="product-name">Product 1</span>
                            <FaCirclePlus className="icon me-2 plus-icon" />
                        </li>
                        <li>
                            <span className="product-name">Product 2</span>
                            <FaCirclePlus className="icon me-2 plus-icon" />
                        </li>
                        <li>
                            <span className="product-name">Product 3</span>
                            <FaCirclePlus className="icon me-2 plus-icon" />
                        </li>
                        <li>
                            <span className="product-name">Product 4</span>
                            <FaCirclePlus className="icon me-2 plus-icon" />
                        </li>
                        <li>
                            <span className="product-name">Product 5</span>
                            <FaCirclePlus className="icon me-2 plus-icon" />
                        </li>
                        <li>
                            <span className="product-name">Product 6</span>
                            <FaCirclePlus className="icon me-2 plus-icon" />
                        </li>
                    </ul>
                </Col>
                <Col className="border ms-5 me-4">
                    <h4 className="mb-3 mt-2 ms-2">Added Products</h4>
                    <ul className="added-product-list  ms-2 me-2">
                        <li>
                            <span className="product-name">Added Product 1</span>
                            <FaMinusCircle className="icon me-2 minus-icon" />
                        </li>
                        <li>
                            <span className="product-name">Added Product 2</span>
                            <FaMinusCircle className="icon me-2 minus-icon" />
                        </li>
                        <li>
                            <span className="product-name">Added Product 3</span>
                            <FaMinusCircle className="icon me-2 minus-icon" />
                        </li>
                        <li>
                            <span className="product-name">Added Product 4</span>
                            <FaMinusCircle className="icon me-2 minus-icon" />
                        </li>
                    </ul>
                </Col>
            </Row>
        </Col>
    );
}
