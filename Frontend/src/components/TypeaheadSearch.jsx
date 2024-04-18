import React from "react";
import { AsyncTypeahead } from "react-bootstrap-typeahead";
import { Col } from "react-bootstrap";

export default function TypeaheadSearch({
    id,
    labelKey,
    options,
    onSearch,
    renderMenuItemChildren,
    ref,
}) {
    return (
        <Col sm={12} lg={6} md={6}>
            <AsyncTypeahead
                className="autocomplete"
                id={id}
                labelKey={labelKey}
                minLength={3}
                options={options}
                onSearch={onSearch}
                renderMenuItemChildren={renderMenuItemChildren}
                ref={ref}
            ></AsyncTypeahead>
        </Col>
    );
}
