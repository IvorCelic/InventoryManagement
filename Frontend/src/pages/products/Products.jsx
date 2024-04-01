import { Container, Table, Button } from "react-bootstrap";
import { RoutesNames } from "../../constants";
import SearchAndAdd from "../../components/SearchAndAdd";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import ProductService from "../../services/ProductService";
import { FaEdit, FaTrash } from "react-icons/fa";

export default function Products() {
    const [products, setProducts] = useState();
    const navigate = useNavigate();

    async function fetchProducts() {
        await ProductService.get()
            .then((res) => {
                setProducts(res.data);
            })
            .catch((error) => {
                alert(error);
            });
    }

    useEffect(() => {
        fetchProducts();
    });

    async function removeProduct(id) {
        const response = await ProductService.remove(id);
        if (response.ok) {
            alert(response.message.data.message);
            fetchProducts();
        }
    }

    function isUnitary(product) {
        if (product.isUnitary == null) return "Not defined";
        if (product.isUnitary) return "Yes";
        return "No";
    }

    return (
        <Container>
            <Container>
                <SearchAndAdd RouteName={RoutesNames.PRODUCTS_CREATE} entity={"product"} />
            </Container>
            <Table striped bordered hover responsive>
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Description</th>
                        <th>Is Unitary</th>
                        <th>Details</th>
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
                                <td>
                                    <Button
                                        onClick={() => {
                                            navigate(`/products/details/${product.id}`);
                                        }}
                                    />
                                </td>
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
        </Container>
    );
}
