import { Container, Table, Button, Col, Row } from "react-bootstrap";
import { RoutesNames } from "../../constants";
import SearchAndAdd from "../../components/SearchAndAdd";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import ProductService from "../../services/ProductService";
import { FaEdit, FaTrash } from "react-icons/fa";
import useError from "../../hooks/useError";
import MyPagination from "../../components/MyPagination";
import useLoading from "../../hooks/useLoading";

export default function Products() {
    const navigate = useNavigate();
    const { showError } = useError();

    const [products, setProducts] = useState();
    const [totalProducts, setTotalProducts] = useState();
    const [page, setPage] = useState(1);
    const [condition, setCondition] = useState("");
    const { showLoading, hideLoading } = useLoading();

    async function fetchProducts() {
        showLoading();
        const responsePagination = await ProductService.GetPagination(page, condition);
        const responseProduct = await ProductService.get("Product");
        if (!responsePagination.ok) {
            showError(responsePagination.data);
            return;
        }

        if (responsePagination.data.length == 0) {
            setPage(page - 1);
            return;
        }
        setProducts(responsePagination.data);
        setTotalProducts(responseProduct.data.length);
        hideLoading();
    }

    useEffect(() => {
        fetchProducts();
    }, [page, condition]);

    async function removeProduct(id) {
        showLoading();
        const response = await ProductService.remove("Product", id);
        showError(response.data);
        if (response.ok) {
            fetchProducts();
        }
        hideLoading();
    }

    function isUnitary(product) {
        if (product.isUnitary == null) return "Not defined";
        if (product.isUnitary) return "Yes";
        return "No";
    }

    const totalPages = Math.ceil(totalProducts / 8);

    function handlePageChange(page) {
        setPage(page);
    }

    function handleSearch(searchTerm) {
        setPage(1);
        setCondition(searchTerm);
    }

    return (
        <Container>
            <Col>
                <Row>
                    <SearchAndAdd
                        RouteName={RoutesNames.PRODUCTS_CREATE}
                        entity={"product"}
                        onSearch={handleSearch}
                    />
                </Row>
                <Row>
                    <Table striped bordered hover responsive>
                        <thead>
                            <tr>
                                <th>Name</th>
                                <th>Description</th>
                                <th>Is Unitary</th>
                                {/* <th>Details</th> */}
                                <th>Action</th>
                            </tr>
                        </thead>
                        <tbody>
                            {products &&
                                products.map((product, index) => (
                                    <tr key={index}>
                                        <td>{product.productName}</td>
                                        <td>{product.description}</td>
                                        <td>{isUnitary(product)}</td>
                                        {/* <td>
                                    <Button
                                        onClick={() => {
                                            navigate(`/products/details/${product.id}`);
                                        }}
                                    />
                                </td> */}
                                        <td>
                                            <Container className="d-flex justify-content-center">
                                                <Button
                                                    variant="link"
                                                    className="me-2 actionButton"
                                                    onClick={() => {
                                                        navigate(`/products/${product.id}`);
                                                    }}
                                                >
                                                    <FaEdit size={25} />
                                                </Button>
                                                <Button
                                                    variant="link"
                                                    className="link-danger actionButton"
                                                    onClick={() => removeProduct(product.id)}
                                                >
                                                    <FaTrash size={25} />
                                                </Button>
                                            </Container>
                                        </td>
                                    </tr>
                                ))}
                        </tbody>
                    </Table>
                </Row>
                <Row>
                    <MyPagination
                        currentPage={page}
                        totalPages={totalPages}
                        onPageChange={handlePageChange}
                    />
                </Row>
            </Col>
        </Container>
    );
}
