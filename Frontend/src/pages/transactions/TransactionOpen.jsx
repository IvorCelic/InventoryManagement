import React, { useEffect, useState } from "react";
import { Col, Form, Row, Table } from "react-bootstrap";
import { FaCirclePlus } from "react-icons/fa6";
import { FaMinusCircle } from "react-icons/fa";
import TransactionItemService from "../../services/TransactionItemService";
import QuantityModal from "../../components/QuantityModal";
import { useParams } from "react-router-dom";
import useError from "../../hooks/useError";
import WarehouseService from "../../services/WarehouseService";
import ProductService from "../../services/ProductService";

export default function TransactionClosed() {
    const routeParams = useParams();
    const { showError } = useError();

    const [warehouses, setWarehouses] = useState([]);
    const [warehouseId, setWarehouseId] = useState(null);
    const [selectedWarehouse, setSelectedWarehouse] = useState(null);
    const [products, setProducts] = useState([]);
    const [associatedWarehouses, setAssociatedWarehouses] = useState([]);
    const [associatedProducts, setAssociatedProducts] = useState([]);
    const [productsOnWarehouse, setProductsOnWarehouse] = useState([]);

    async function fetchWarehouses() {
        const response = await WarehouseService.get("Warehouse");
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setWarehouses(response.data);
    }

    async function fetchProducts() {
        const response = await ProductService.get("Product");
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setProducts(response.data);
        console.table(response.data);
    }

    async function fetchAssociatedProducts() {
        const response = await TransactionItemService.GetProducts(
            "InventoryTransactionItem",
            routeParams.id
        );
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setAssociatedProducts(response.data);
        // console.table(response.data);
    }

    async function fetchAssociatedWarehouses() {
        const response = await TransactionItemService.GetWarehouses(
            "InventoryTransactionItem",
            routeParams.id
        );
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setAssociatedWarehouses(response.data);
        // console.log("warehouses: ", response.data);
    }

    async function fetchProductsOnWarehouse(warehouseId) {
        const response = await TransactionItemService.GetProductsOnWarehouse(
            "InventoryTransactionItem",
            routeParams.id,
            warehouseId
        );
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setProductsOnWarehouse(response.data);
    }

    useEffect(() => {
        fetchInitialData();
    }, []);

    async function fetchInitialData() {
        await fetchWarehouses();
        await fetchProducts();
        await fetchAssociatedProducts();
        await fetchAssociatedWarehouses();
        await fetchProductsOnWarehouse();
    }

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
                {" "}
                <Col>
                    <Form.Group className="mb-3 pt-2 ms-2 me-3" controlId="warehouse">
                        <Form.Label>Warehouse</Form.Label>
                        <Form.Select onChange={handleWarehouseChange}>
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
                <Row className="mt-3">
                    <Col>
                        <h5 className="mt-3">Currently added products:</h5>
                        <Table striped bordered hover size="sm" className="table-sm">
                            <thead>
                                <tr>
                                    <th>Name</th>
                                    <th>Quantity</th>
                                </tr>
                            </thead>
                            <tbody>
                                {associatedProducts &&
                                    associatedProducts.map((product, index) => (
                                        <tr key={index}>
                                            <td>{product.productName}</td>
                                            <td>{product.quantity}</td>
                                        </tr>
                                    ))}
                            </tbody>
                        </Table>
                    </Col>
                </Row>
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
                                            // onClick={() =>
                                            //     handleAddProduct(product.id, product.isUnitary)
                                            // }
                                        />
                                    </li>
                                ))}
                        </ul>
                    </Col>
                    <Col className="border ms-5 me-4">
                        <h4 className="mb-3 mt-2 ms-2">Added Products</h4>
                        {productsOnWarehouse && productsOnWarehouse.length > 0 ? (
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
                                                // onClick={() => removeProductOnWarehouse(item.id)}
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
