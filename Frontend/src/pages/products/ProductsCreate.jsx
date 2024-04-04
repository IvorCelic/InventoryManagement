import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import { RoutesNames } from "../../constants";
import ProductService from "../../services/ProductService";
import useError from "../../hooks/useError";
import ActionButtons from "../../components/ActionButtons";

export default function ProductsCreate() {
    const navigate = useNavigate();
    const entityName = "product";
    const { showError } = useError();

    async function addProduct(entityName) {
        const response = await ProductService.add("Product", entityName);
        if (response.ok) {
            navigate(RoutesNames.PRODUCTS_LIST);
            return;
        }
        showError(response.data);
    }

    function handleSubmit(event) {
        event.preventDefault();
        const data = new FormData(event.target);

        addProduct({
            productName: data.get("productname"),
            description: data.get("description"),
            isUnitary: data.get("isunitary") == "on" ? true : false,
        });
    }

    return (
        <Container>
            <Container className="square border mt-5">
                <h2 className="mt-5 ms-5">Add new {entityName}</h2>
                <Form className="m-5" onSubmit={handleSubmit}>
                    <Form.Group controlId="productName">
                        <Form.Label>Product name</Form.Label>
                        <Form.Control placeholder="Product name" type="text" name="productname" />
                    </Form.Group>
                    <Form.Group controlId="description">
                        <Form.Label className="pt-4">Description</Form.Label>
                        <Form.Control
                            placeholder="Product description"
                            type="text"
                            name="description"
                        />
                    </Form.Group>
                    <Form.Group controlId="isUnitary">
                        <Form.Check label="Is Unitary" name="isunitary" />
                    </Form.Group>
                    <ActionButtons cancel={RoutesNames.PRODUCTS_LIST} action="Add product" />
                </Form>
            </Container>
        </Container>
    );
}
