import { Container, Form } from "react-bootstrap";
import { useNavigate, useParams } from "react-router-dom";
import { RoutesNames } from "../../constants";
import ProductService from "../../services/ProductService";
import { useEffect, useState } from "react";
import useError from "../../hooks/useError";
import ActionButtons from "../../components/ActionButtons";

export default function ProductsEdit() {
    const navigate = useNavigate();
    const routeParams = useParams();
    const [product, setProduct] = useState({});
    const entityName = "product";
    const { showError } = useError();
    const [showModal, setShowModal] = useState();

    async function fetchProduct() {
        const response = await ProductService.getById("Product", routeParams.id);
        if (!response.ok) {
            showError(response.data);
            navigate(RoutesNames.PRODUCTS_LIST);
            return;
        }
        setProduct(response.data);
        setShowModal(false);
    }

    useEffect(() => {
        fetchProduct();
    }, []);

    async function editProduct(entityName) {
        const response = await ProductService.edit("Product", routeParams.id, entityName);
        if (response.ok) {
            navigate(RoutesNames.PRODUCTS_LIST);
            return;
        } else {
            console.log(response);
            showError(response.data);
        }
    }

    function handleSubmit(event) {
        event.preventDefault();
        const data = new FormData(event.target);

        editProduct({
            productName: data.get("productname"),
            description: data.get("description"),
            isUnitary: data.get("isunitary") == "on" ? true : false,
        });
    }

    return (
        <Container>
            <Container className="square border mt-5">
                <h2 className="mt-5 ms-5">Edit {entityName}</h2>
                <Form className="m-5" onSubmit={handleSubmit}>
                    <Form.Group controlId="productName">
                        <Form.Label>Product name</Form.Label>
                        <Form.Control
                            type="text"
                            defaultValue={product.productName}
                            name="productname"
                        />
                    </Form.Group>
                    <Form.Group controlId="description">
                        <Form.Label className="pt-4">Description</Form.Label>
                        <Form.Control
                            type="text"
                            defaultValue={product.description}
                            name="description"
                        />
                    </Form.Group>
                    <Form.Group controlId="isUnitary">
                        <Form.Check
                            label="Is Unitary"
                            defaultChecked={product.isUnitary}
                            name="isunitary"
                        />
                    </Form.Group>
                    <ActionButtons cancel={RoutesNames.PRODUCTS_LIST} action="Edit product" />
                </Form>
            </Container>
        </Container>
    );
}
