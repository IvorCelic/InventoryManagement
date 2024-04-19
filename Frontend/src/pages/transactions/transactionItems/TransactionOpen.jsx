import React, { useEffect, useRef, useState } from "react";
import { Col, Form, Row, Table } from "react-bootstrap";
import { FaCirclePlus } from "react-icons/fa6";
import { FaMinusCircle } from "react-icons/fa";
import TransactionItemService from "../../../services/TransactionItemService";
import QuantityModal from "../../../components/QuantityModal";
import { useParams } from "react-router-dom";
import useError from "../../../hooks/useError";
import WarehouseService from "../../../services/WarehouseService";
import { AsyncTypeahead } from "react-bootstrap-typeahead";
import useLoading from "../../../hooks/useLoading";
import MyPagination from "../../../components/MyPagination";

export default function TransactionOpen() {
    const routeParams = useParams();
    const { showError } = useError();
    const { showLoading, hideLoading } = useLoading();
    const typeaheadRefLeft = useRef(null);
    const typeaheadRefRight = useRef(null);

    const [warehouses, setWarehouses] = useState([]);
    const [warehouseId, setWarehouseId] = useState(null);
    const [products, setProducts] = useState([]);

    const [selectedWarehouse, setSelectedWarehouse] = useState(null);
    const [selectedProductId, setSelectedProductId] = useState(null);

    const [associatedProducts, setAssociatedProducts] = useState([]);
    const [productsOnWarehouse, setProductsOnWarehouse] = useState([]);
    const [showModal, setShowModal] = useState(false);
    const [foundUnassociatedProducts, setFoundUnassociatedProducts] = useState([]);
    const [foundProductsOnWarehouse, setFoundProductOnWarehouse] = useState([]);

    const [pagePonW, setPagePonW] = useState(1);
    const [pageUnP, setPageUnP] = useState(1);
    const [totalProductsOnWarehouse, setTotalProductsOnWarehouse] = useState();
    const [totalUnassociatedProducts, setTotalUnassociatedProducts] = useState();

    async function fetchWarehouses() {
        showLoading();
        const response = await WarehouseService.get("Warehouse");
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setWarehouses(response.data);
        hideLoading();
    }

    async function fetchProducts() {
        showLoading();
        const responsePagination = await TransactionItemService.GetUnassociatedProductsPagination(
            routeParams.id,
            pageUnP
        );
        const response = await TransactionItemService.GetUnassociatedProducts(
            "InventoryTransactionItem",
            routeParams.id
        );
        if (!response.ok) {
            showError(response.data);
            return;
        }
        if (responsePagination.data.length == 0) {
            setPageUnP(pageUnP - 1);
            setTotalUnassociatedProducts(response.data.length);
            return;
        }
        setProducts(responsePagination.data);
        setTotalUnassociatedProducts(response.data.length);
        // console.table(response.data);
        hideLoading();
    }

    async function fetchAssociatedProducts() {
        showLoading();
        const response = await TransactionItemService.GetProducts(
            "InventoryTransactionItem",
            routeParams.id
        );
        if (!response.ok) {
            showError(response.data);
            return;
        }
        // console.table(response.data);
        setAssociatedProducts(response.data);
        hideLoading();
    }

    async function fetchAssociatedWarehouses() {
        showLoading();
        const response = await TransactionItemService.GetWarehouses(
            "InventoryTransactionItem",
            routeParams.id
        );
        if (!response.ok) {
            showError(response.data);
            return;
        }
        // console.log("warehouses: ", response.data);
        hideLoading();
    }

    async function fetchProductsOnWarehouse(warehouseId) {
        showLoading();
        const responsePagination = await TransactionItemService.GetProductsOnWarehousePagination(
            routeParams.id,
            warehouseId,
            pagePonW
        );
        const response = await TransactionItemService.GetProductsOnWarehouse(
            "InventoryTransactionItem",
            routeParams.id,
            warehouseId
        );
        if (!response.ok) {
            showError(response.data);
            return;
        }
        if (responsePagination.data.length == 0) {
            setPagePonW(pagePonW - 1);
            setTotalProductsOnWarehouse(0);
            return;
        }
        setProductsOnWarehouse(responsePagination.data);
        setTotalProductsOnWarehouse(response.data.length);
        hideLoading();
    }

    async function addProductsOnWarehouse(productId, quantity) {
        showLoading();
        const entity = {
            transactionId: parseInt(routeParams.id),
            warehouseId: parseInt(warehouseId),
            productId: productId,
            quantity: quantity,
        };

        // console.log(entity);
        const response = await TransactionItemService.add("InventoryTransactionItem", entity);
        if (response.ok) {
            await fetchProductsOnWarehouse(warehouseId);
            await fetchProducts();
            typeaheadRefLeft.current.clear();
            return;
        }

        showError(response.data);
        hideLoading();
    }

    async function searchUnassociatedProduct(condition) {
        const response = await TransactionItemService.SearchUnassociatedProduct(
            "InventoryTransactionItem",
            routeParams.id,
            condition
        );
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setFoundUnassociatedProducts(response.data);
    }

    async function searchProductOnWarehouse(condition) {
        const response = await TransactionItemService.SearchProductOnWarehouse(
            "InventoryTransactionItem",
            routeParams.id,
            warehouseId,
            condition
        );
        console.log("ERROR: ->>>>>>>>>>>>>>>,   ", response.data);
        if (!response.ok) {
            showError(response.data);
            return;
        }
        setFoundProductOnWarehouse(response.data);
    }

    useEffect(() => {
        fetchInitialData();
    }, [pageUnP, pagePonW]);

    async function fetchInitialData() {
        await fetchWarehouses();
        await fetchProducts();
        await fetchAssociatedProducts();
        await fetchAssociatedWarehouses();

        if (warehouseId !== null) {
            await fetchProductsOnWarehouse(warehouseId);
        }
    }

    function handleAddProduct(productId, isUnitary) {
        if (isUnitary) {
            setSelectedProductId(productId);
            setShowModal(true);
        } else {
            addProductsOnWarehouse(productId, 1);
        }
    }

    async function handleAddWithQuantity(customQuantity) {
        addProductsOnWarehouse(selectedProductId, customQuantity);
        setShowModal(false);
        await fetchProducts();
    }

    function nameWarehouse() {
        for (let i = 0; i < warehouses.length; i++) {
            const entity = warehouses[i];
            if (entity.id == warehouseId) {
                return entity.warehouseName;
            }
        }
    }

    async function removeProductOnWarehouse(id) {
        const response = await TransactionItemService.remove("InventoryTransactionItem", id);
        showError(response.data);
        if (response.ok) {
            await fetchProductsOnWarehouse(warehouseId);
            await fetchProducts();
            typeaheadRefRight.current.clear();
            return;
        }
        // console.log("ID OF INVENTORYTRANSACTIONITEM", id);
    }

    const handleWarehouseChange = async (event) => {
        const selectedWarehouseId = event.target.value;
        setSelectedWarehouse(selectedWarehouseId);

        if (selectedWarehouseId === "") {
            setWarehouseId(null);
            await fetchAssociatedProducts();
        } else {
            setWarehouseId(selectedWarehouseId);
            await fetchProductsOnWarehouse(selectedWarehouseId);
        }
    };

    const totalPagesUnP = Math.ceil(totalUnassociatedProducts / 8);
    const totalPagesPonW = Math.ceil(totalProductsOnWarehouse / 8);

    function handlePageChangePonW(page) {
        setPagePonW(page);
    }

    function handlePageChangeUnP(page) {
        setPageUnP(page);
    }

    return (
        <Col lg={8} md={12} sm={12} className="border mt-5 transactionEditContainer">
            <Row className="align-items-center">
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
                    <Row className="mb-3">
                        <Col>
                            <Form.Group controlId="condition">
                                <Form.Label>Add by searching</Form.Label>
                                <AsyncTypeahead
                                    className="autocomplete"
                                    id="condition"
                                    emptyLabel="No result"
                                    searchText="Searching..."
                                    labelKey={(product) => `${product.productName}`}
                                    minLength={3}
                                    options={foundUnassociatedProducts}
                                    onSearch={searchUnassociatedProduct}
                                    placeholder="Part of product name"
                                    renderMenuItemChildren={(product) => (
                                        <div>
                                            <span>{product.productName}</span>
                                            <FaCirclePlus
                                                className="icon ms-2 plus-icon"
                                                onClick={() =>
                                                    handleAddProduct(product.id, product.isUnitary)
                                                }
                                            />
                                        </div>
                                    )}
                                    ref={typeaheadRefLeft}
                                />
                            </Form.Group>
                        </Col>
                        <Col>
                            <Form.Group controlId="condition">
                                <Form.Label>Remove by searching</Form.Label>
                                <AsyncTypeahead
                                    className="autocomplete"
                                    id="condition"
                                    emptyLabel="No result"
                                    searchText="Searching..."
                                    labelKey={(product) => `${product.productName}`}
                                    minLength={3}
                                    options={foundProductsOnWarehouse}
                                    onSearch={searchProductOnWarehouse}
                                    placeholder="Part of product name"
                                    renderMenuItemChildren={(product) => (
                                        <div>
                                            <span>{product.productName}</span>
                                            <FaMinusCircle
                                                className="icon me-2 minus-icon"
                                                onClick={() => removeProductOnWarehouse(product.id)}
                                            />
                                        </div>
                                    )}
                                    ref={typeaheadRefRight}
                                />
                            </Form.Group>
                        </Col>
                    </Row>
                    <Row>
                        <Col className="border me-4 ms-4">
                            <Row>
                                <Col>
                                    {products.length > 0 ? (
                                        <>
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
                                                                        product.id,
                                                                        product.isUnitary
                                                                    )
                                                                }
                                                            />
                                                        </li>
                                                    ))}
                                            </ul>
                                        </>
                                    ) : (
                                        <p className="mt-2">All products have been added.</p>
                                    )}
                                </Col>
                            </Row>
                            <Row>
                                <Col className="d-flex justify-content-center">
                                    <MyPagination
                                        currentPage={pageUnP}
                                        totalPages={totalPagesUnP}
                                        onPageChange={handlePageChangeUnP}
                                    />
                                </Col>
                            </Row>
                        </Col>
                        <Col className="border me-4 ms-4">
                            <Row>
                                <Col>
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
                                                            onClick={() =>
                                                                removeProductOnWarehouse(item.id)
                                                            }
                                                        />
                                                    </li>
                                                ))}
                                        </ul>
                                    ) : (
                                        <p>There are no products on this warehouse.</p>
                                    )}
                                </Col>
                            </Row>
                            <Row>
                                <Col className="d-flex justify-content-center">
                                    <MyPagination
                                        currentPage={pagePonW}
                                        totalPages={totalPagesPonW}
                                        onPageChange={handlePageChangePonW}
                                    />
                                </Col>
                            </Row>
                        </Col>
                    </Row>
                    <Row></Row>
                </Row>
            )}
            <QuantityModal
                show={showModal}
                handleClose={() => setShowModal(false)}
                handleAdd={handleAddWithQuantity}
            />
        </Col>
    );
}
