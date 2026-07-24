<div align="center">

# The Mercury Project

### Mercury RC 1.2

**Version 1.2.0-rc.1**  
**CSE499 Release Candidate**

</div>

---

## Release Information

| Release | Version | Status |
| :--- | :--- | :--- |
| **Mercury RC 1.2** | **1.2.0-rc.1** | **CSE499 Release Candidate** |

## Team Quotes

| Team Member | Quote |
| :--- | :--- |
| **Matthew Barker** | “Some people spend an entire lifetime wondering if they made a difference in the world. But, the Marines don't have that problem.”<br><br>**Ronald Reagan** |
| **Kim Brown** | “It always seems impossible until it's done.”<br><br>**Nelson Mandela** |

## CSE499 Senior Project

Mercury is a proof-of-concept secure communications framework designed to demonstrate applied software engineering, modular architecture, defensive programming, and security-focused design.

The project builds and demonstrates a reusable secure data exchange layer for distributed software systems. Mercury protects payloads, detects tampering, rejects replay attempts, and separates security logic from transport logic.

> **Academic Scope**  
> Mercury is not a chat application, commercial product submission, military system, or DoD-certified or production-certified system. It is an academic prototype built to demonstrate senior-level software engineering through working code, clear architecture, automated tests, and documented design decisions.

---

## Table of Contents

| Project Definition | Security and Architecture | Build, Release, and Documentation |
| :--- | :--- | :--- |
| [Project Summary](#project-summary)<br>[Problem Statement](#problem-statement)<br>[Project Goals](#project-goals)<br>[Academic Scope](#academic-scope)<br>[Demonstrated Capabilities](#demonstrated-capabilities)<br>[Demo Scenarios](#demo-scenarios) | [Security Objectives](#security-objectives)<br>[Threat Model](#threat-model)<br>[Architecture Overview](#architecture-overview)<br>[Solution Structure](#solution-structure)<br>[Communication Flow](#communication-flow)<br>[Core Components](#core-components)<br>[Cryptographic Design](#cryptographic-design)<br>[Replay Protection](#replay-protection)<br>[Transport Design](#transport-design) | [Build Instructions](#build-instructions)<br>[Test Instructions](#test-instructions)<br>[Running the Demo](#running-the-demo)<br>[Release Status](#release-status)<br>[Known Limitations](#known-limitations)<br>[Future Improvement Work](#future-improvement-work)<br>[Documentation](#documentation)<br>[Copyright, Confidentiality, and Academic Use](#copyright-confidentiality-and-academic-use)<br>[Summary](#summary) |

---

## Project Summary

Mercury provides a secure communications layer for software components that need to exchange protected data over potentially untrusted transports.

The framework protects application payloads before transmission and validates protected payloads before releasing unprotected data to the receiving application. It is designed around separation of responsibility:

- Applications provide payloads.
- Mercury protects and validates those payloads.
- Transports move protected frames.
- Crypto providers handle encryption and authentication.
- Codecs serialize and deserialize secure envelopes.
- Replay protection prevents duplicate protected communications from being accepted.

Mercury demonstrates how a secure communications layer can be designed in a modular, testable, and extensible way.

---

## Problem Statement

Distributed software systems often exchange data across communication channels that may not be fully trusted. Data may pass through memory queues, TCP connections, network services, relays, files, serial links, or other transports. If payload protection is handled incorrectly, the system may be exposed to serious risks.

Common risks include:

- Unauthorized reading of payload contents
- Payload tampering
- Replay of previously captured protected frames
- Incorrect key usage
- Unsafe failure behavior
- Security logic tightly coupled to transport logic
- Developer misuse caused by unclear or unsafe APIs

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
11. Provide working demos that prove the core security properties.
12. Provide automated tests that validate core framework behavior.

---

## Academic Scope

Mercury is submitted as a CSE499 senior project.

The project is presented as a general-purpose secure communications framework for distributed software systems. It is intended to demonstrate architecture, implementation, documentation, modularity, testing, and defensive programming.

This project is not:

- A military system
- A classified system
- A DoD-certified product
- A production-certified security product
- A commercial product submission
- A complete identity management system
- A replacement for mature enterprise security infrastructure
- A public key infrastructure implementation
- A certificate authority implementation
- A production key management system

The project focuses on senior-level software engineering and security design principles, not on certification or production deployment claims.

---

## Security Objectives

Mercury demonstrates the following security properties:

| Security Property | Description |
| --- | --- |
| Confidentiality | Payload contents are encrypted before transport. |
| Integrity | Modified protected communications are rejected. |
| Authentication | Protected communications are validated using authenticated encryption and authenticated envelope context. |
| Replay Protection | Previously accepted replay-token combinations are rejected. |
| Safe Failure | Invalid protected communications fail without exposing unprotected data. |
| Transport Independence | Security is applied above the transport layer. |
| Modular Design | Crypto, transport, encoding, replay protection, and core coordination are separated. |

---

## Threat Model

### Threat Model Summary

Mercury assumes that protected frames may be transmitted over transports that are not fully trusted. An attacker may be able to observe, copy, modify, drop, delay, or replay transmitted data.

The framework is designed to protect the payload and reject invalid protected communications before unprotected data is released to the receiving application.

Mercury does not assume that the transport is secure.

Mercury does assume that cryptographic keys are provisioned correctly before use.

---

### Assets Protected

The primary assets protected by Mercury are:

| Asset | Description |
| --- | --- |
| Payload Contents | The application data being exchanged. |
| Payload Integrity | Assurance that the protected payload was not modified undetected. |
| Sender/Recipient Context | Cryptographic context identifying the intended protected exchange. |
| Replay Token Uniqueness | Values used to detect duplicate protected communications. |
| SecureEnvelope | The structured protected container used during transport. |
| Unprotected Data Release | The point where validated data becomes available to the application. |

---

### Trust Boundaries

#### Application Boundary

The application is responsible for providing payloads and selecting the appropriate sender and recipient context. Mercury is responsible for protecting the payload after it is submitted.

#### Framework Boundary

Mercury is responsible for secure-envelope creation, cryptographic protection, replay validation, decoding, and safe failure behavior.

#### Transport Boundary

The transport is not trusted to provide confidentiality, integrity, or authenticity. The transport only moves protected frames.

#### Cryptographic Provider Boundary

The crypto provider is responsible for encryption, authentication, and validation of protected data.

#### Key Management Boundary

The current version assumes keys already exist and are made available through a key provider. Long-term key management, certificate chains, identity proofing, and key rotation are outside the scope of this version.

---

### Assumptions

The framework assumes:

1. The selected cryptographic algorithm is implemented correctly by the underlying platform.
2. Keys are generated with sufficient entropy.
3. Keys are stored and provided securely outside the current framework scope.
4. Sender and recipient key identifiers are assigned consistently.
5. Replay-protection state is available for the lifetime of the receiving context.
6. The application treats failed receive results as rejected communications.
7. The transport may be hostile, unreliable, or observable.
8. Attackers do not already possess the correct secret key material.

---

### Attacker Capabilities

The threat model assumes an attacker may be able to:

- Observe transmitted frames
- Capture protected frames
- Replay previously captured frames
- Modify protected payload bytes
- Modify authenticated envelope fields
- Send malformed frames
- Attempt to decrypt with the wrong key
- Attempt to confuse sender or recipient context
- Drop frames
- Delay frames
- Send duplicate frames
- Attempt to trigger unsafe exception behavior

The threat model does not assume that the attacker can:

- Break AES-GCM or ChaCha20-Poly1305 cryptography
- Access valid secret key material
- Modify trusted application memory
- Compromise the operating system
- Compromise the .NET runtime
- Compromise the developer's machine
- Defeat all possible side-channel attacks

---

### Threats and Mitigations

| Threat | Description | Mitigation |
| --- | --- | --- |
| Eavesdropping | An attacker observes transmitted frames. | Payload encryption prevents unprotected data exposure. |
| Payload Tampering | An attacker modifies protected payload bytes. | Authenticated encryption rejects modified protected data. |
| Envelope Tampering | An attacker changes authenticated header or footer data. | Canonical envelope data is included in authenticated encryption. |
| Replay Attack | An attacker resends a previously captured protected frame. | Replay tokens are tracked and duplicates are rejected. |
| Wrong Key Usage | A receiver attempts to decrypt with incorrect key material. | Authentication validation fails and unprotected data is not released. |
| Malformed Frame | An attacker sends invalid or corrupted frame data. | Codec and validation logic reject malformed input. |
| Transport Compromise | The transport is observable or unreliable. | Security is applied above the transport layer. |
| Sender/Recipient Confusion | An attacker attempts to alter communications context. | Sender, recipient, algorithm, replay, and metadata context are included in authenticated envelope data. |
| Unsafe Failure | Invalid input causes uncontrolled behavior or data exposure. | The framework returns controlled failure results and avoids data release on failure. |
| Duplicate Delivery | A valid frame is delivered multiple times. | Replay protection rejects previously accepted replay-token combinations. |
| Provider Misuse | A provider returns invalid protected data. | Core validation checks secure envelopes before encoding or processing. |

---

### Out of Scope Threats

The following threats are intentionally out of scope for this version:

- Public key infrastructure attacks
- Certificate authority compromise
- Long-term key rotation failure
- Hardware security module integration
- Full identity proofing
- User account compromise
- Operating system compromise
- Runtime compromise
- Memory scraping by malware
- Side-channel attacks
- Traffic analysis
- Denial-of-service certification
- Formal cryptographic verification
- Post-quantum cryptography
- Production deployment hardening
- Regulatory or government certification

These items are valid security concerns, but they are outside the academic scope of this version.

---

### Security Design Position

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

At a high level, the application submits a payload to the framework. The Mercury client passes the payload and cryptographic context to the selected crypto provider. The provider creates a protected SecureEnvelope, the selected codec encodes it, and the selected transport moves the resulting frame.

On the receiving side, Mercury receives the protected frame, decodes the SecureEnvelope, validates and decrypts the protected content, checks replay protection, and releases unprotected data only if the communication passes all security checks.

High-level flow:

```text
Application
   ↓
Mercury Client
   ↓
Crypto Provider
   ↓
SecureEnvelope
   ↓
Envelope Codec
   ↓
Transport
   ↓
Receiving Mercury Client
   ↓
Application
```

The transport does not decide whether a protected communication is secure. It only moves frames.

The codec does not provide cryptographic security. It serializes and deserializes SecureEnvelope structures.

The crypto provider does not decide how protected frames are delivered. It protects and validates payloads and authenticated envelope data.

This separation allows Mercury to support multiple transports, codecs, and crypto providers without rewriting the core communications pipeline.

---

## Solution Structure

The solution is organized into separate projects:

| Project | Purpose |
| --- | --- |
| Mercury.Abstractions | Interfaces, contracts, shared models, primitives, and framework abstractions. |
| Mercury.Core | Main communications pipeline, validation, replay protection, client logic, and coordination. |
| Mercury.Provider.AesGcm | AES-GCM authenticated-encryption provider. |
| Mercury.Provider.ChaCha20 | ChaCha20-Poly1305 authenticated-encryption provider. |
| Mercury.Transport.InMemory | In-memory transport for demonstrations and automated tests. |
| Mercury.Transport.Tcp | Framed TCP transport implementation. |
| Mercury.Transport.FileDrop | File-based transport plugin. |
| Mercury.Demo.Console | Cross-platform console demonstration application. |
| Mercury.Demo.WinForms | Windows graphical demonstration application. |
| Mercury.Tests | Automated xUnit tests validating framework behavior. |

AES-CCM remains pending and is not claimed as a completed RC 1.2 provider.

---

## Communication Flow

### Send Flow

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
Protect Payload and Authenticated Envelope Data
   ↓
Validate SecureEnvelope
   ↓
Encode SecureEnvelope
   ↓
Send Frame Through Transport
```

Mercury does not send unprotected payloads through the transport.

### Receive Flow

The receive pipeline is:

```text
Receive Frame From Transport
   ↓
Decode SecureEnvelope
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

### Mercury Client

The client coordinates the secure communications pipeline. It is responsible for:

- Building protection requests
- Calling the crypto provider
- Encoding protected envelopes
- Sending frames through transport
- Receiving frames from transport
- Decoding protected envelopes
- Calling the crypto provider for validation and decryption
- Applying replay protection
- Returning controlled results

### Crypto Provider

The crypto provider is responsible for:

- Encrypting payloads
- Authenticating protected payloads and envelope context
- Producing SecureEnvelope instances
- Validating and decrypting SecureEnvelope instances
- Rejecting tampered or unauthorized protected communications

### Codec

The codec is responsible for converting SecureEnvelope instances to and from transportable byte frames.

The codec provides structural validation but does not provide cryptographic integrity. Cryptographic integrity is enforced by the selected authenticated-encryption provider.

Mercury RC 1.2 includes:

- Binary envelope codec
- JSON envelope codec

### Transport

The transport is responsible for moving byte frames.

The transport does not provide confidentiality, integrity, authentication, or replay protection. Those responsibilities remain inside Mercury.

### Replay Protector

The replay protector tracks sender and replay-token combinations and rejects duplicates.

The in-memory replay protector uses a configurable replay window, removes expired entries, limits the number of active replay records, and fails closed when its configured capacity is reached.

Replay state is process-local and is lost when the application restarts.

---

## Cryptographic Design

Mercury RC 1.2 includes two operational authenticated-encryption providers:

- AES-GCM
- ChaCha20-Poly1305

Both providers implement the same Mercury cryptographic-provider contract and can be selected without changing the core client, codec, or transport pipeline.

AES-CCM remains pending and is not claimed as a completed RC 1.2 provider.

Authenticated encryption provides:

- Payload confidentiality
- Payload integrity
- Authentication-tag validation
- Tamper detection
- Wrong-key rejection
- Authentication of canonical envelope data

Mercury uses authenticated encryption so modified protected data cannot be decrypted and released as valid unprotected data.

### Key Handling

The current version assumes keys are already available through an `ISymmetricKeyProvider` implementation.

The in-memory dictionary provider:

- Rejects empty key identifiers
- Rejects null or empty key material
- Defensively copies supplied key arrays
- Returns copied key material
- Honors cancellation
- Rejects unknown key identifiers

Algorithm-specific providers validate the key length required by their selected algorithm.

This version does not implement:

- Key-generation workflows
- Key-exchange protocols
- Public/private key identity
- Certificate validation
- Key rotation
- Hardware-backed key storage

Those are future-work items and are not claimed as current capabilities.

### Authentication Scope

Mercury authenticates the protected payload together with canonical envelope data.

The authenticated envelope data includes:

- Framework version
- Envelope identifier
- Timestamp
- Sender key identifier
- Recipient key identifier
- Encryption algorithm identifier
- Signature algorithm identifier
- Replay token
- Header metadata
- Footer metadata

Changing any authenticated field causes authentication failure, and unprotected payload data is not released.

This does not claim human identity verification, certificate-based authentication, or digital-signature-based identity.

---

## Replay Protection

Replay attacks occur when an attacker captures a valid protected frame and sends it again later.

Mercury mitigates replay attacks by assigning and tracking replay tokens. If a sender and replay-token combination has already been accepted, the protected communication is rejected.

The in-memory replay protector:

- Uses a configurable replay window
- Removes expired entries
- Enforces a configurable maximum number of active entries
- Rejects additional entries when capacity is reached
- Handles concurrent duplicate submissions safely

Replay protection is scoped to the replay protector instance and its stored state. Persistent replay storage is identified as future work.

---

## Transport Design

Mercury treats transports as untrusted delivery mechanisms.

The current solution includes:

- In-memory transport for tests and demonstrations
- Framed TCP transport
- FileDrop transport plugin

The transport layer is intentionally separated from the security layer. This allows the same security pipeline to be used with different transports.

Potential future transports could include:

- Named pipes
- WebSockets
- Queues
- Serial links
- HTTP-based relays

Mercury allows transport expansion without changing the core cryptographic pipeline.

---

## Demonstrated Capabilities

Mercury RC 1.2 demonstrates:

- Protected payload transmission
- AES-GCM authenticated encryption
- ChaCha20-Poly1305 authenticated encryption
- Binary and JSON SecureEnvelope encoding
- In-memory and TCP transport
- FileDrop transport integration
- Sender and recipient key context
- Wrong-key rejection
- Protected-payload tamper rejection
- Authenticated-envelope-field tamper rejection
- Replay detection
- Controlled failure behavior
- Maximum TCP and in-memory frame limits
- Bounded in-memory replay tracking
- Cross-platform console demonstration
- Windows WinForms demonstration
- Automated xUnit test coverage

---

## Demo Scenarios

The demonstrations support the following scenarios:

1. Send and receive a protected text payload.
2. Select AES-GCM or ChaCha20-Poly1305.
3. Select Binary or JSON envelope encoding.
4. Use supported transports available in the selected demo.
5. Capture and replay a previously accepted protected frame.
6. Modify a protected frame in transit.
7. Confirm that replayed or tampered communications are rejected.
8. Confirm that unprotected payload data is released only after successful validation.

The TCP attack simulator is a demonstration component. It is not part of the Mercury transport or cryptographic provider implementation.

---

## Build Instructions

### Requirements

- .NET SDK 10
- Windows for the WinForms demonstration
- macOS, Windows, or Linux for the console demonstration and compatible framework projects

### Build

From the solution root:

```powershell
dotnet restore
dotnet build
```

A clean release build can be created with:

```powershell
dotnet build --configuration Release
```

---

## Test Instructions

Run the complete automated test suite from the solution root:

```powershell
dotnet test
```

Run a release-configuration verification with:

```powershell
dotnet test --configuration Release
```

The release candidate should not be tagged unless the full suite completes with zero failed tests.

---

## Running the Demo

### Console Demo

From the solution root:

```powershell
dotnet run --project Mercury.Demo.Console
```

The console demonstration is intended to provide a cross-platform Mercury host.

### WinForms Demo

On Windows:

```powershell
dotnet run --project Mercury.Demo.WinForms
```

The WinForms application provides the primary graphical demonstration, including supported provider, codec, transport, tamper, and replay scenarios.

---

## Release Status

Mercury RC 1.2 is feature-complete for the defined CSE499 scope.

Latest verification:

- 330 automated tests discovered
- 328 tests passed
- 2 AES-CCM tests skipped
- 0 tests failed
- WinForms demonstration verified
- AES-GCM verified
- ChaCha20-Poly1305 verified
- Binary and JSON codecs verified
- In-memory and TCP transport scenarios verified
- Tamper and replay demonstrations verified

The two skipped tests are associated with the pending AES-CCM provider and are not failures in the completed RC 1.2 providers.

---

## Known Limitations

Mercury RC 1.2 is a working academic release candidate. It demonstrates secure-framework design and core security concepts, but it is not presented as production-ready software.

Known limitations include:

- No public key infrastructure
- No certificate-based identity model
- No production key management system
- No formal key exchange protocol
- No long-term key rotation
- No formal cryptographic proof
- No side-channel hardening
- No production deployment hardening
- Process-local in-memory replay protection
- No production-scale fuzz testing or long-duration stress certification
- Demo-focused user interfaces
- AES-CCM provider remains pending
- FileDrop recovery and production hardening remain limited compared with the primary in-memory and TCP paths

These limitations are documented intentionally so the project does not overclaim its current capabilities.

---

## Future Improvement Work

The following items are outside the required CSE499 RC 1.2 scope:

- Digital signatures
- Certificate-based identity
- Public key exchange
- Persistent replay protection storage
- Additional transport providers
- Expanded malformed-input and fuzz testing
- Benchmark and long-duration stress testing
- Protocol version negotiation
- Key rotation support
- Secure key storage integration
- Hardware security module integration
- Key-memory zeroing and disposal where practical
- WPF demonstration client
- Production deployment guidance

---

## Documentation

Formal project documentation is maintained separately from this README.

Planned documentation set:

- [Mercury Requirements](Docs/Mercury_Requirements.docx)
- [Mercury Design](Docs/Mercury_Design.docx)
- [Mercury Threat Model](Docs/Mercury_Threat_Model.docx)
- [Mercury Test Plan](Docs/Mercury_Test_Plan_and_Report.docx)
- [Mercury User Manual](Docs/Mercury_User_Manual.docx)
- [Mercury Final Report](Docs/Mercury_Final_Report.docx)

The README provides the project overview, release status, build instructions, test instructions, and operating summary. The formal documents provide the detailed academic requirements, architecture, threat model, test evidence, user guidance, and final project analysis.

---

## Copyright, Confidentiality, and Academic Use

Copyright © 2026 Matthew D. Barker. All rights reserved.

Mercury is submitted as an academic senior project for CSE499 review and grading purposes only.

This submission does not grant permission to publish, redistribute, sublicense, publicly display, publicly post, or otherwise distribute the source code, documentation, architecture, diagrams, screenshots, binaries, or related project materials outside the course review process.

Authorized course personnel may review the submitted materials for academic evaluation. No commercial license, public distribution right, open-source license, or production-use permission is granted.

Any public presentation, showcase, publication, repository posting, redistribution, or external sharing of this project requires prior written permission from the copyright owner.

---

## Summary

Mercury demonstrates a secure communications framework with modular cryptography, transport abstraction, SecureEnvelope protection, replay protection, tamper detection, automated tests, and working console and WinForms demonstrations.

The project is designed to show senior-level software engineering through architecture, implementation, testing, documentation, and defensive design.

The mission is straightforward:

Protect the payload.  
Keep the design clean.  
Treat transport as untrusted.  
Fail safely.  
Prove the system works.
