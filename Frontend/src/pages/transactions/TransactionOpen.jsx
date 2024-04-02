import React, { useEffect, useState } from "react";
import { Col, Form, Row } from "react-bootstrap";
import { FaCirclePlus } from "react-icons/fa6";
import { FaMinusCircle } from "react-icons/fa";
import TransactionItemService from "../../services/TransactionItemService";

export default function TransactionOpen({
    warehouses,
    products,
    warehouseId,
    setWarehouseId,
    productsOnWarehouse,
    transactionId,
    handleProductOnoWarehouseChange,
    isUnitary,
}) {
    const [selectedWarehouse, setSelectedWarehouse] = useState(null);
    const [quantity, setQuantity] = useState(1);

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

    useEffect(() => {
        return isUnitary === true ? setQuantity(1) : setQuantity(2);
    });

    const handleAddProduct = async (transactionId, warehouseId, productId, quantity) => {
        try {
            const response = await TransactionItemService.AddProductOnWarehouse(
                parseInt(transactionId),
                parseInt(warehouseId),
                productId,
                quantity
            );

            console.log("quantity:", quantity);
            console.log(response);
            handleProductOnoWarehouseChange();
        } catch (error) {
            console.log(error);
        }
    };

    async function removeProductOnWarehouse(id) {
        const response = await TransactionItemService.remove(id);
        if (response.ok) {
            alert(response.message.data.message);
            handleProductOnoWarehouseChange();
        } else {
            console.log(response.message);
        }
    }

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
                                        <FaCirclePlus
                                            className="icon me-2 plus-icon"
                                            onClick={() =>
                                                handleAddProduct(
                                                    transactionId,
                                                    warehouseId,
                                                    product.id,
                                                    quantity
                                                )
                                            }
                                        />
                                    </li>
                                ))}
                        </ul>
                    </Col>
                    <Col className="border ms-5 me-4">
                        <h4 className="mb-3 mt-2 ms-2">Added Products</h4>
                        {productsOnWarehouse.length > 0 ? (
                            <ul className="product-list ms-2 me-2">
                                {productsOnWarehouse &&
                                    productsOnWarehouse.map((item, index) => (
                                        <li key={index}>
                                            <span className="product-name ms-2">
                                                {item.productName}
                                            </span>
                                            <span>{item.quantity}</span>
                                            <FaMinusCircle
                                                className="icon me-2 minus-icon"
                                                onClick={() => removeProductOnWarehouse(item.id)}
                                            />
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
