import { Button, Col, Container, Form, Row } from "react-bootstrap";
import { Link, useNavigate, useParams } from "react-router-dom";
import { RoutesNames } from "../../constants";
import ProductService from "../../services/ProductService";
import { useEffect, useState } from "react";

export default function ProductsEdit() {
    const navigate = useNavigate();
    const routeParams = useParams();
    const [product, setProduct] = useState({});
    const entityName = "product";

    async function fetchProduct() {
        await ProductService.getById(routeParams.id)
            .then((res) => {
                setProduct(res.data);
            })
            .catch((error) => {
                alert(error.message);
            });
    }

    useEffect(() => {
        fetchProduct();
    }, []);

    async function editProduct(entityName) {
        const response = await ProductService.edit(routeParams.id, entityName);
        if (response.ok) {
            navigate(RoutesNames.PRODUCTS_LIST);
        } else {
            console.log(response);
            alert(response.message);
        }
    }

    function handleSubmit(error) {
        error.preventDefault();
        const data = new FormData(error.target);

        const entityName = {
            productName: data.get("productname"),
            description: data.get("description"),
            isUnitary: data.get("isunitary") == "on" ? true : false
        };

        editProduct(entityName);
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
                            required
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
                    <Row className="mb-0 flex-column flex-sm-row">
                        <Col className="d-flex align-items-center mb-2 mb-sm-0">
                            <Link className="btn btn-danger myButton" to={RoutesNames.PRODUCTS_LIST}>
                                Cancel
                            </Link>
                        </Col>
                        <Col className="d-flex align-items-center">
                            <Button className="myButton" variant="primary" type="submit">
                                Save changes
                            </Button>
                        </Col>
                    </Row>
                </Form>
            </Container>
        </Container>
    );
}
