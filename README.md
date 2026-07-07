# The Mercury Project - This document is a work in progress

## Team Quotes

- Matthew Barker: “Some people spend an entire lifetime wondering if they made a difference in the world. But, the Marines don't have that problem.”  
  — Ronald Reagan

- Kim Brown: "It always seems impossible until it's done."
  - Nelson Mandela

## CSE499 Senior Project

Mercury is a proof-of-concept secure communications framework designed to demonstrate applied software engineering, modular architecture, defensive programming, and security-focused design.

The purpose of this project is to build and demonstrate a reusable secure data exchange layer for distributed software systems. Mercury protects payloads, detects tampering, rejects replay attempts, and separates security logic from transport logic.

This project is not a chat application. It is not a commercial product submission. It is not a military system. It is not presented as a DoD-certified or production-certified system.

Mercury is an academic prototype built to demonstrate senior-level software engineering through working code, clear architecture, automated tests, and documented design decisions.

---

## Table of Contents

* [Project Summary](#project-summary)
* [Problem Statement](#problem-statement)
* [Project Goals](#project-goals)
* [Academic Scope](#academic-scope)
* [Security Objectives](#security-objectives)
* [Threat Model](#threat-model)
* [Architecture Overview](#architecture-overview)
* [Solution Structure](#solution-structure)
* [Communication Flow](#communication-flow)
* [Core Components](#core-components)
* [Cryptographic Design](#cryptographic-design)
* [Replay Protection](#replay-protection)
* [Transport Design](#transport-design)
* [Demonstrated Capabilities](#demonstrated-capabilities)
* [Demo Scenarios](#demo-scenarios)
* [Build Instructions](#build-instructions)
* [Test Instructions](#test-instructions)
* [Running the Demo](#running-the-demo)
* [Known Limitations](#known-limitations)
* [Future Work](#future-work)
* [Documentation](#documentation)
* [Copyright, Confidentiality, and Academic Use](#copyright-confidentiality-and-academic-use)
* [Summary](#summary)

---

## Project Summary

Mercury provides a secure communications layer for software components that need to exchange protected data over potentially untrusted transports.

The framework protects application payloads before transmission and validates protected payloads before releasing unprotected data to the receiving application. It is designed around separation of responsibility:

* Applications provide payloads.
* Mercury protects and validates those payloads.
* Transports move protected frames.
* Crypto providers handle encryption and authentication.
* Replay protection prevents duplicate protected communications from being accepted.

Mercury demonstrates how a secure communications layer can be designed in a modular, testable, and extensible way.

---

## Problem Statement

Distributed software systems often exchange data across communication channels that may not be fully trusted. Data may pass through memory queues, TCP connections, network services, relays, files, serial links, or other transports. If payload protection is handled incorrectly, the system may be exposed to serious risks.

Common risks include:

* Unauthorized reading of payload contents
* Payload tampering
* Replay of previously captured protected frames
* Incorrect key usage
* Unsafe failure behavior
* Security logic tightly coupled to transport logic
* Developer misuse caused by unclear or unsafe APIs

Mercury addresses these risks by providing a reusable secure communications layer that protects payloads independently of the transport used to move them.

---

## Project Goals

The project goals are:

1. Demonstrate a secure communications and data exchange framework.
2. Protect payload confidentiality.
3. Detect tampering before unprotected data is released.
4. Validate protected communications using authenticated encryption.
5. Reject replayed protected communications.
6. Separate cryptographic logic from transport logic.
7. Support interchangeable transport providers.
8. Support interchangeable cryptographic providers.
9. Provide a developer-facing API that encourages safe usage.
10. Fail safely when protected communications are invalid, corrupted, replayed, or unauthorized.
11. Provide a working demo that proves the core security properties.
12. Provide automated tests that validate core framework behavior.

---

## Academic Scope

Mercury is submitted as a CSE499 senior project.

The project is presented as a general-purpose secure communications framework for distributed software systems. It is intended to demonstrate architecture, implementation, documentation, modularity, testing, and defensive programming.

This project is not:

* A military system
* A classified system
* A DoD-certified product
* A production-certified security product
* A commercial product submission
* A complete identity management system
* A replacement for mature enterprise security infrastructure
* A public key infrastructure implementation
* A certificate authority implementation
* A production key management system

The project focuses on senior-level software engineering and security design principles, not on certification or production deployment claims.

---

## Security Objectives

Mercury demonstrates the following security properties:

| Security Property      | Description                                                                            |
| ---------------------- | -------------------------------------------------------------------------------------- |
| Confidentiality        | Payload contents are encrypted before transport.                                       |
| Integrity              | Modified protected communications are rejected.                                        |
| Authentication         | Protected communications are validated using authenticated encryption and key-bound context. |
| Replay Protection      | Previously seen replay tokens are rejected.                                            |
| Safe Failure           | Invalid protected communications fail without exposing unprotected data.               |
| Transport Independence | Security is applied above the transport layer.                                         |
| Modular Design         | Crypto, transport, encoding, and replay protection are separated.                      |

---

# Threat Model

## Threat Model Summary

Mercury assumes that protected frames may be transmitted over transports that are not fully trusted. An attacker may be able to observe, copy, modify, drop, delay, or replay transmitted data.

The framework is designed to protect the payload and reject invalid protected communications before unprotected data is released to the receiving application.

Mercury does not assume that the transport is secure.

Mercury does assume that cryptographic keys are provisioned correctly before use.

---

## Assets Protected

The primary assets protected by Mercury are:

| Asset                    | Description                                                          |
| ------------------------ | -------------------------------------------------------------------- |
| Payload Contents         | The application data being exchanged.                                |
| Payload Integrity        | Assurance that the protected payload was not modified undetected.    |
| Sender/Recipient Context | Cryptographic context identifying the intended protected exchange.   |
| Replay Token Uniqueness  | Values used to detect duplicate protected communications.            |
| Protected Envelope       | The structured container used during transport.                      |
| Unprotected Data Release | The point where validated data becomes available to the application. |

---

## Trust Boundaries

Mercury uses the following trust boundaries:

### Application Boundary

The application is responsible for providing payloads and selecting the appropriate sender and recipient context. Mercury is responsible for protecting the payload after it is submitted.

### Framework Boundary

Mercury is responsible for secure envelope creation, cryptographic protection, replay validation, decoding, and safe failure behavior.

### Transport Boundary

The transport is not trusted to provide confidentiality, integrity, or authenticity. The transport only moves protected frames.

### Cryptographic Provider Boundary

The crypto provider is responsible for encryption, authentication, and validation of protected data.

### Key Management Boundary

The current version assumes keys already exist and are made available through a key provider. Long-term key management, certificate chains, identity proofing, and key rotation are outside the scope of this version.

---

## Assumptions

The framework assumes:

1. The selected cryptographic algorithm is implemented correctly by the underlying platform.
2. Keys are generated with sufficient entropy.
3. Keys are stored and provided securely outside the current framework scope.
4. Sender and recipient key identifiers are assigned consistently.
5. Replay protection state is available for the lifetime of the receiving context.
6. The application treats failed receive results as rejected communications.
7. The transport may be hostile, unreliable, or observable.
8. Attackers do not already possess the correct secret key material.

---

## Attacker Capabilities

The threat model assumes an attacker may be able to:

* Observe transmitted frames
* Capture protected frames
* Replay previously captured frames
* Modify protected payload bytes
* Modify frame bytes
* Send malformed frames
* Attempt to decrypt with the wrong key
* Attempt to confuse sender/recipient context
* Drop frames
* Delay frames
* Send duplicate frames
* Attempt to trigger unsafe exception behavior

The threat model does not assume that the attacker can:

* Break AES-GCM cryptography
* Access valid secret key material
* Modify trusted application memory
* Compromise the operating system
* Compromise the .NET runtime
* Compromise the developer's machine
* Defeat all possible side-channel attacks

---

## Threats and Mitigations

| Threat                     | Description                                                      | Mitigation                                                                                |
| -------------------------- | ---------------------------------------------------------------- | ----------------------------------------------------------------------------------------- |
| Eavesdropping              | An attacker observes transmitted frames.                         | Payload encryption prevents unprotected data exposure.                                    |
| Payload Tampering          | An attacker modifies protected payload bytes.                    | Authenticated encryption rejects modified protected data.                                 |
| Replay Attack              | An attacker resends a previously captured protected frame.       | Replay tokens are tracked and duplicates are rejected.                                    |
| Wrong Key Usage            | A receiver attempts to decrypt with incorrect key material.      | Authentication validation fails and unprotected data is not released.                     |
| Malformed Frame            | An attacker sends invalid or corrupted frame data.               | Codec and validation logic reject malformed input.                                        |
| Transport Compromise       | The transport is observable or unreliable.                       | Security is applied above the transport layer.                                            |
| Sender/Recipient Confusion | An attacker attempts to alter communications context.            | Cryptographic context is bound into authentication logic where supported.                 |
| Unsafe Failure             | Invalid input causes uncontrolled behavior or data exposure.     | The framework returns controlled failure results and avoids data release on failure.      |
| Duplicate Delivery         | A valid frame is delivered multiple times.                       | Replay protection rejects previously accepted replay tokens.                              |
| Provider Misuse            | A provider returns invalid protected data.                       | Core validation checks protected envelopes before encoding or processing.                 |

---

## Out of Scope Threats

The following threats are intentionally out of scope for this version:

* Public key infrastructure attacks
* Certificate authority compromise
* Long-term key rotation failure
* Hardware security module integration
* Full identity proofing
* User account compromise
* Operating system compromise
* Runtime compromise
* Memory scraping by malware
* Side-channel attacks
* Traffic analysis
* Denial of service
* Formal cryptographic verification
* Post-quantum cryptography
* Production deployment hardening
* Regulatory or government certification

These items are valid security concerns, but they are outside the academic scope of this version.

---

## Security Design Position

Mercury is designed to demonstrate modern secure communications handling principles:

1. Protect data before transport.
2. Treat transport as untrusted.
3. Authenticate protected data before releasing unprotected data.
4. Reject replayed protected communications.
5. Keep security logic separate from delivery logic.
6. Fail safely.
7. Avoid overclaiming security properties not implemented by the current version.

---

## Architecture Overview

Mercury is built around separation of responsibility.

At a high level, the application submits a payload to the framework. Mercury creates a protected envelope, encrypts and authenticates the payload, encodes the protected envelope, and sends it through a transport.

On the receiving side, Mercury receives the protected frame, decodes the protected envelope, validates the protected content, checks replay protection, and releases unprotected data only if the protected communication passes security validation.

High-level flow:

```text
Application
   ↓
Mercury Client
   ↓
Secure Envelope
   ↓
Crypto Provider
   ↓
Codec
   ↓
Transport
   ↓
Receiver
```

The transport does not decide whether a protected communication is secure. It only moves frames.

The crypto provider does not decide how protected frames are delivered. It only protects and validates payload content.

This separation allows Mercury to support multiple transports and crypto providers without rewriting the core communications pipeline.

---

## Solution Structure

The solution is organized into separate projects:

| Project                         | Purpose                                                                  |
| ------------------------------- | ------------------------------------------------------------------------ |
| Mercury.Abstractions            | Interfaces, contracts, shared models, and framework abstractions.        |
| Mercury.Core                    | Main communications pipeline, client logic, validation, and coordination. |
| Mercury.Providers.AesGcm        | AES-GCM cryptographic provider implementation.                           |
| Mercury.Transport.InMemory      | In-memory transport for demos and automated tests.                       |
| Mercury.Transport.Tcp           | TCP transport implementation using framed communication.                 |
| Mercury.Demo.WinForms           | Graphical demonstration application.                                     |
| Mercury.Tests                   | Automated tests validating core behavior.                                |

---

## Communication Flow

## Send Flow

The send pipeline is:

```text
Application Payload
   ↓
Build Crypto Context
   ↓
Validate Context
   ↓
Create Protection Request
   ↓
Protect Payload
   ↓
Validate Protected Envelope
   ↓
Encode Envelope
   ↓
Send Frame Through Transport
```

Mercury does not send unprotected payloads through the transport.

## Receive Flow

The receive pipeline is:

```text
Receive Frame From Transport
   ↓
Decode Protected Envelope
   ↓
Validate Envelope Structure
   ↓
Unprotect and Authenticate
   ↓
Check Replay Token
   ↓
Release Validated Payload
```

Unprotected data is released only after successful validation.

---

## Core Components

## Mercury Client

The client coordinates the secure communications pipeline. It is responsible for:

* Building protection requests
* Calling the crypto provider
* Encoding protected envelopes
* Sending frames through transport
* Receiving frames from transport
* Decoding protected envelopes
* Calling the crypto provider for validation/decryption
* Applying replay protection
* Returning controlled receive results

## Crypto Provider

The crypto provider is responsible for:

* Encrypting payloads
* Authenticating protected data
* Producing protected envelopes
* Validating protected envelopes
* Rejecting tampered or unauthorized protected communications

## Codec

The codec is responsible for converting protected envelopes to and from transportable byte frames.

The codec does not provide security. It only serializes and deserializes protected envelope structures.

## Transport

The transport is responsible for moving byte frames.

The transport does not provide confidentiality, integrity, authentication, or replay protection. Those responsibilities remain inside Mercury.

## Replay Protector

The replay protector tracks replay tokens and rejects duplicates.

Replay protection prevents a previously captured protected frame from being accepted more than once in the same replay-protection context.

---

## Cryptographic Design

The current cryptographic provider uses AES-GCM.

AES-GCM provides authenticated encryption, which supports:

* Payload confidentiality
* Payload integrity
* Authentication tag validation
* Tamper detection
* Wrong-key rejection

Mercury uses authenticated encryption so modified protected data cannot be decrypted and released as valid unprotected data.

## Key Handling

The current version assumes keys are already available through a key provider.

This version does not implement:

* Key generation workflows
* Key exchange protocols
* Public/private key identity
* Certificate validation
* Key rotation
* Hardware-backed key storage

Those are future-work items and are not claimed as current capabilities.

## Authentication Scope

The current version demonstrates authentication through authenticated encryption and key-bound context.

This means the receiver can validate that the protected data was created using expected key material and context. It does not claim full human identity verification, certificate-based authentication, or public/private signature-based identity.

---

## Replay Protection

Replay attacks occur when an attacker captures a valid protected frame and sends it again later.

Mercury mitigates replay attacks by assigning and tracking replay tokens. If a replay token has already been accepted, the protected communication is rejected.

Replay protection is scoped to the replay protector instance and its stored state.

The current version uses in-memory replay tracking. This is appropriate for the academic prototype and demo. Persistent replay storage is identified as future work.

---

## Transport Design

Mercury treats transports as untrusted delivery mechanisms.

The current version includes:

* In-memory transport for tests and demos
* TCP transport for real framed data exchange

The transport layer is intentionally separated from the security layer. This allows the same security pipeline to be used with different transports.

Potential future transports could include:

* Named pipes
* WebSockets
* Queues
* Files
* Serial links
* HTTP-based relays

Mercury allows transport expansion without changing the core cryptographic pipeline.

---

## Known Design Limitations

Mercury version 0.5x is a working academic prototype. It demonstrates secure framework design and core security concepts, but it is not presented as production-ready software.

Known limitations include:

* No public key infrastructure
* No certificate-based identity model
* No production key management system
* No formal key exchange protocol
* No long-term key rotation
* No formal cryptographic proof
* No side-channel hardening
* No production deployment hardening
* In-memory replay protection only
* Limited metadata authentication
* Demo-focused user interface
* Limited automated test coverage compared to production security software

These limitations are documented intentionally so the project does not overclaim its current capabilities.

---

## Future Improvment Work - Out of scope of CSE499

Future improvements may include:

* Digital signatures
* Certificate-based identity
* Public key exchange
* Persistent replay protection storage
* Authenticated metadata canonicalization
* Additional transport providers
* Expanded automated test coverage
* Benchmark testing
* Protocol version negotiation
* Key rotation support
* Secure key storage integration
* Console demo client
* WPF demo client
* Production deployment guidance

---

## Documentation - Coming Soon

Formal project documentation is maintained separately from this README.

Expected documentation set:

```text
/docs/Mercury_Requirements.docx
/docs/Mercury_Design.docx
/docs/Mercury_Threat_Model.docx
/docs/Mercury_Test_Plan.docx
/docs/Mercury_User_Manual.docx
/docs/Mercury_Final_Report.docx
```

The README provides the project overview and operating summary. The Word documents provide formal academic detail.

---

## Copyright, Confidentiality, and Academic Use

Copyright © 2026 Matthew D. Barker. All rights reserved.

Mercury is submitted as an academic senior project for CSE499 review and grading purposes only.

This submission does not grant permission to publish, redistribute, sublicense, publicly display, publicly post, or otherwise distribute the source code, documentation, architecture, diagrams, screenshots, binaries, or related project materials outside the course review process.

Authorized course personnel may review the submitted materials for academic evaluation. No commercial license, public distribution right, open-source license, or production-use permission is granted.

Any public presentation, showcase, publication, repository posting, redistribution, or external sharing of this project requires prior written permission from the copyright owner.

---

## Summary

Mercury demonstrates a secure communications framework with modular cryptography, transport abstraction, protected envelopes, replay protection, tamper detection, automated tests, and a working demo.

The project is designed to show senior-level software engineering through architecture, implementation, testing, documentation, and defensive design.

The mission is straightforward:

Protect the payload.
Keep the design clean.
Treat transport as untrusted.
Fail safely.
Prove the system works.
