import React, { useState } from "react";
import { Col, Form, Row } from "react-bootstrap";
import { FaCirclePlus } from "react-icons/fa6";
import { FaMinusCircle } from "react-icons/fa";

export default function TransactionOpen({
    warehouses,
    products,
    warehouseId,
    setWarehouseId,
    productsOnWarehouse,
}) {
    const [selectedWarehouse, setSelectedWarehouse] = useState(null);

    function nameWarehouse() {
        for (let i = 0; i < warehouses.length; i++) {
            const entity = warehouses[i];
            if (entity.id == warehouseId) {
                return entity.warehouseName;
            }
        }
    }

    const handleWarehouseChange = async (event) => {
        const selectedWarehouseId = event.target.value;
        setSelectedWarehouse(selectedWarehouseId);
        setWarehouseId(selectedWarehouseId);
    };

    return (
        <Col lg={8} md={12} sm={12} className="border mt-5 transactionEditContainer">
            <Row className="align-items-center">
                <Col>
                    <Form.Group className="mb-3 pt-2 ms-2 me-3" controlId="warehouse">
                        <Form.Label>Warehouse</Form.Label>
                        <Form.Select value={warehouseId} onChange={handleWarehouseChange}>
                            <option value="">Select warehouse</option>
                            {warehouses &&
                                warehouses.map((warehouse, index) => (
                                    <option key={index} value={warehouse.id}>
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
            {!selectedWarehouse ? (
                ""
            ) : (
                <Row className="mt-4">
                    <Col className="border me-4 ms-4">
                        <h4 className="mb-3 mt-2 ms-2">Products</h4>
                        <ul className="product-list ms-2 me-2">
                            {products &&
                                products.map((product, index) => (
                                    <li key={index}>
                                        <span className="product-name ms-2">
                                            {product.productName}
                                        </span>
                                        <FaCirclePlus className="icon me-2 plus-icon" />
                                    </li>
                                ))}
                        </ul>
                    </Col>
                    <Col className="border ms-5 me-4">
                        <h4 className="mb-3 mt-2 ms-2">Added Products</h4>
                        {productsOnWarehouse.length > 0 ? (
                            <ul className="product-list ms-2 me-2">
                                {productsOnWarehouse &&
                                    productsOnWarehouse.map((product, index) => (
                                        <li key={index}>
                                            <span className="product-name ms-2">
                                                {product.productName}
                                            </span>
                                            <span>{product.quantity}</span>
                                            <FaMinusCircle className="icon me-2 minus-icon" />
                                        </li>
                                    ))}
                            </ul>
                        ) : (
                            <p>There are no products on this warehouse.</p>
                        )}
                    </Col>
                </Row>
            )}
        </Col>
    );
}
